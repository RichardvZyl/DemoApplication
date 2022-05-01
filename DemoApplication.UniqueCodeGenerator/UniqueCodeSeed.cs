using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.AspNetCore;
using DemoApplication.Database;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DemoApplication.UniqueCodeGenerator;

public class UniqueCodeSeed : IUniqueCodeSeed
{
    private static string _databaseConnectionString { get; set; } = string.Empty;

    private readonly IServiceCollection _serviceCollection;

    public UniqueCodeSeed(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    #region Main Calling function
    public Task<bool> Start(IServiceCollection serviceCollection)
    {
        _databaseConnectionString = serviceCollection.GetConnectionString(nameof(Context));

        try
        {
            CreateSchema();
            CreateTable();
            CreateFunction();
            SeedCodes();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Task.FromResult(false);
        }
    }
    #endregion

    #region Private Members
    private const int FirstCode = 100000;
    private const long LastCode = 999999;
    private const int AmountToGenerate = 100;
    private static readonly Random random = new();
    #endregion

    #region MainFunctionality
    private static void SeedCodes()
        => Insert(CreateInsertString(GenerateRandom(AmountToGenerate, FirstCode, LastCode)));//Environment.Exit(1);
    #endregion

    private static string CreateInsertString(List<long> codes)
    {
        var values = codes.Aggregate("", (current, code) => current + $"('{code}'),");

        values = values.Remove(values.Length - 1, 1);

        return @$"INSERT INTO ""CODE"".""GeneratedCodes"" (code) VALUES {values};";
    }

    private static void Insert(string sql)
    {
        using (var conn = new NpgsqlConnection(_databaseConnectionString))
        {
            conn.Open();

            var command = new NpgsqlCommand(sql, conn);

            command.ExecuteScalar();

            conn.Close();
        }
    }

    private static void CreateTable()
    {
        using (var conn = new NpgsqlConnection(_databaseConnectionString))
        {
            conn.Open();

            const string table = @"
                CREATE TABLE IF NOT EXISTS ""CODE"".""GeneratedCodes"" ( 
                    id INT GENERATED ALWAYS AS IDENTITY,
                    code VARCHAR NOT NULL,
                    CONSTRAINT ""uniqueCodes"" PRIMARY KEY(code)
                );";

            var command = new NpgsqlCommand(table, conn);

            command.ExecuteNonQuery();

            conn.Close();
        }
    }

    private static void CreateSchema()
    {
        using (var conn = new NpgsqlConnection(_databaseConnectionString))
        {
            conn.Open();

            const string table = @"CREATE SCHEMA IF NOT EXISTS ""CODE""";

            NpgsqlCommand command = new NpgsqlCommand(table, conn);

            command.ExecuteNonQuery();

            conn.Close();
        }
    }

    private static void CreateFunction()
    {
        using (var conn = new NpgsqlConnection(_databaseConnectionString))
        {
            conn.Open();

            const string proc = @"
                    CREATE OR REPLACE FUNCTION GetGeneratedCode()
                    RETURNS 
                    TABLE ( code VARCHAR ) AS
                    $$
                    BEGIN
                        RETURN QUERY EXECUTE (
                            'DELETE FROM ""CODE"".""GeneratedCodes""
                            WHERE ""code"" IN
                            (
                                SELECT ""code""
                                FROM ""CODE"".""GeneratedCodes""
                                --ORDER BY ID
                                LIMIT 1
                                FOR UPDATE SKIP LOCKED
                            )
                            RETURNING ""code""; ');
                    END;
                    $$
                    LANGUAGE plpgsql;";

            var command = new NpgsqlCommand(proc, conn);

            _ = command.ExecuteNonQuery();

            conn.Close();
        }
    }

    #region Rules

    private static bool HasThreeRepeating(string code)
    {
        for (var i = 0; i < 10; i++)
        {
            if (code.Contains($"{i}{i}{i}"))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasConsecutiveA(string code)
    {
        for (var i = 0; i < 8; i++)
        {
            if (code.Contains($"{i}{i + 1}{i + 2}"))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasConsecutiveD(string code)
    {
        for (var i = 2; i < 10; i++)
        {
            if (code.Contains($"{i}{i - 1}{i - 2}"))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region RNG Algoritm
    private static List<long> GenerateRandom(int count, int min, long max)
    {
        if (max <= min || count < 0 || (count > max - min && max - min > 0))
        {
            throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                    " (" + (max - min) + " values), or count " + count + " is illegal");
        }

        static long LongRandom(long min, long max, Random rand)
        {
            var buf = new byte[8];
            rand.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        static bool RuleBroken(string code)
            => HasThreeRepeating(code) || HasConsecutiveA(code) || HasConsecutiveD(code);

        var candidates = new HashSet<long>();

        for (var top = max - count; top < max; top++)
        {
            var code = LongRandom(min, top + 1, random);

            if (RuleBroken(code.ToString()))
                continue;

            if (candidates.Add(code))
                continue;

            if (RuleBroken(top.ToString()))
                continue;

            _ = candidates.Add(top);
        }

        var result = candidates.ToList();

        for (var i = result.Count - 1; i > 0; i--)
        {
            var k = random.Next(i + 1);
            var tmp = result[k];
            result[k] = result[i];
            result[i] = tmp;
        }

        return result.Select(x => x).Distinct().ToList();
    }
    #endregion
}
