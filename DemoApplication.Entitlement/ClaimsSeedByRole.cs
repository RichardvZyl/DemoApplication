using System.Security.Claims;
using Abstractions.Entitlement;
using DemoApplication.Entitlement;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.ClaimExceptions;

/// <summary> The user default roles definitions - each role has default claims that are assigned to that role (role based entitlement) </summary>
public static class ClaimsSeedByRole
{
    #region Functions
    public static List<Claim> AddClaimsFromRoles(this List<Claim> claims, RolesEnum role)
        => role switch
        {
            RolesEnum.Clerk => ClerkRole(claims),
            RolesEnum.Supervisor => SupervisorRole(claims),
            RolesEnum.Financial => FinancialRole(claims),
            RolesEnum.Interface => InterfaceRole(claims),
            RolesEnum.Administrator => AdministratorRole(claims),
            _ => claims,
        };

    public static EntitlementModel GetDefaultEntitlement(RolesEnum role)
        => role switch
        {
            RolesEnum.Clerk => ClaimsSeedByRole<EntitlementModel>.EntitlementFromClaims(ClerkRole(new List<Claim>())),
            RolesEnum.Supervisor => ClaimsSeedByRole<EntitlementModel>.EntitlementFromClaims(SupervisorRole(new List<Claim>())),
            RolesEnum.Financial => ClaimsSeedByRole<EntitlementModel>.EntitlementFromClaims(FinancialRole(new List<Claim>())),
            RolesEnum.Interface => ClaimsSeedByRole<EntitlementModel>.EntitlementFromClaims(InterfaceRole(new List<Claim>())),
            RolesEnum.Administrator => ClaimsSeedByRole<EntitlementModel>.EntitlementFromClaims(AdministratorRole(new List<Claim>())),
            _ => new EntitlementModel()
        };
    #endregion

    #region Claims By Role
    private static List<Claim> AdministratorRole(List<Claim> claims)
    {
        claims.Add(DemoApplicationClaims.AuditReport);
        claims.Add(DemoApplicationClaims.AuthorizeMakerChecker);
        claims.Add(DemoApplicationClaims.EntitlementChange);
        claims.Add(DemoApplicationClaims.StatisticalReport);
        claims.Add(DemoApplicationClaims.SuspendUsers);
        claims.Add(DemoApplicationClaims.ViewNotifications);
        claims.Add(DemoApplicationClaims.ViewUsers);
        claims.Add(DemoApplicationClaims.AuditLogs);

        return claims;
    }

    private static List<Claim> ClerkRole(List<Claim> claims) //Default new role
    {
        claims.Add(DemoApplicationClaims.ViewNotifications);

        return claims;
    }

    private static List<Claim> FinancialRole(List<Claim> claims)
    {
        claims.Add(DemoApplicationClaims.AuditReport);
        claims.Add(DemoApplicationClaims.AuthorizeMakerChecker);
        claims.Add(DemoApplicationClaims.EntitlementChange);
        claims.Add(DemoApplicationClaims.StatisticalReport);
        claims.Add(DemoApplicationClaims.SuspendUsers);
        claims.Add(DemoApplicationClaims.ViewNotifications);
        claims.Add(DemoApplicationClaims.ViewUsers);

        return claims;
    }

    private static List<Claim> InterfaceRole(List<Claim> claims)
        => claims;

    private static List<Claim> SupervisorRole(List<Claim> claims)
    {//Currently no different from administrator
        claims.Add(DemoApplicationClaims.AuditReport);
        claims.Add(DemoApplicationClaims.StatisticalReport);
        claims.Add(DemoApplicationClaims.AuthorizeMakerChecker);
        claims.Add(DemoApplicationClaims.EntitlementChange);
        claims.Add(DemoApplicationClaims.SuspendUsers);
        claims.Add(DemoApplicationClaims.ViewNotifications);
        claims.Add(DemoApplicationClaims.ViewUsers);
        claims.Add(DemoApplicationClaims.AuditLogs);

        return claims;
    }
    #endregion
}
