import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, EMPTY } from "rxjs";
import { UserModel } from "../models/user/user.model";
import { UserModelNoAuth } from "../models/user/user-noauth.model";
import { ResultModel } from "../models/auth/result.model";
import { NewMakerCheckerModel } from "../models/auth/new-maker-checker.model";
import { LookupEnumModel } from "../models/auth/lookup-enum.model";
import { environment } from "../../environments/environment";

const httpOptions = {
    headers: new HttpHeaders({
      "Content-Type": "application/json"
    })
  }

@Injectable({ providedIn: "root" })

// Class is used to interact with backend pertaining to users
export class AppUserService {
    usersUrl = environment.apiUrl + "Users";
    makerCheckerUrl = environment.apiUrl + "MakerCheckers";
    lookupUrl = environment.apiUrl + "Lookups"

    users: UserModel[] = [];

    constructor(private readonly http: HttpClient) {
        console.log("User Service Constructor");
     }

    //#region part of template
    add(model: UserModel) {
        return this.http.post<number>("Users", model);
    }

    delete(id: number) {
        return this.http.delete(`Users/${id}`);
    }

    get(id: number) {
        return this.http.get<UserModel>(`Users/${id}`);
    }

    list() {
        return this.http.get<UserModel[]>("Users");
    }

    update(model: UserModel) {
        return this.http.put(`Users/${model.id}`, model);
    }
    //#endregion

    // Take user model as it will be available?
    // rather use id as it's less data to pass around?
    // function exists named get but I preffer this naming convention / syntax
    getUser(user: UserModel) : Observable<UserModel> {
        // get a specific user //(expand)
        console.log("getUser called within userService");

        if (!user) {
            console.log("An Error occured as an empty user model was received within getUser function");
            return EMPTY;
        }
        console.log(user);

        return this.http.get<UserModel>(`Users/${user.id}`);
    }

    getCurrentUser() : Promise<UserModel> {
        return this.http.get<UserModel>(`${this.usersUrl}/Current`).toPromise();
    }

    updateUser(patchDoc: string, user: UserModel) : ResultModel {
        // make changes to user
        console.log("updateUser called within userService");

        if (!patchDoc) {
            console.log("An Error occured as an empty userModel was received in updateUser function");
            return new ResultModel();
        }
        console.log(patchDoc);

        this.http.patch(`${this.usersUrl}/${user.id}`, patchDoc, httpOptions).subscribe(
            data => console.log(data),
            error  => alert(error.error ?? error.message)
        );

        return { description: "testing", resultCode: 1 };
    }

    // TODO should this include a request input parameter?
    async getUsers() : Promise<UserModelNoAuth[]> {
        // get all users saved in backend
        console.log("getUsers called within userService");

        // this is/was also an existing function namely 'list' --I preffer this naming convention/ syntax

        this.users = await this.http.get<UserModelNoAuth[]>(this.usersUrl).toPromise();

        if (!this.users) {
            console.log("No users received from the API");
            return [];
        }

        return this.users;
    }

    async getUserTypes() : Promise<string[]> {
        const keyValuePair = await this.http.get<LookupEnumModel[]>(`${this.lookupUrl}/RolesEnum`).toPromise();
        const roles: string[] = [];

        keyValuePair.forEach(keyValue => {
            roles.push(`${keyValue.value} ${keyValue.key}`)
        });

        return roles;
    }


    suspendUser(suspendUserRequest: NewMakerCheckerModel) : ResultModel {
        // suspend received user
        console.log("suspendUser called within userService");

        if (!suspendUserRequest) {
            console.log("An Error occured as an empty SuspendUserRequestModel was received in suspendUser function");
            return new ResultModel();
        }
        console.log(suspendUserRequest);

        this.http.post(`${this.makerCheckerUrl}/SuspendUser/`, suspendUserRequest, httpOptions).subscribe(
            data => console.log(data),
            error  => alert(error.error ?? error.message)
        );

        return { description: "testing", resultCode: 1 };
        // return this.http.put(`Users/${user.id}`, user);
    }

    //#region // TODO this also exists in auth service // rather use auth service //
    resetPassword(email: string) : ResultModel {
        // request a password change link email
        console.log("resetPassword called within userService");

        if (!email) {
            console.log("An error occured because an empty email was received within resetPassword function");
            return new ResultModel();
        }
        console.log(email);

        return { description: "testing", resultCode: 1 };
        // return this.http.post<string>(`Users/${email}`, "");
    }

    register(user: UserModel) : ResultModel {
        // request creation of a new user // backend will handle sending a confirmation email and save details accordingly
        console.log("register called within userService");

        if (!user) {
            console.log("An error occured as an empty userModel was received within register method");
            return new ResultModel();
        }
        console.log(user)

        const emptyMakerChecker = new NewMakerCheckerModel();
        emptyMakerChecker.action = 0;
        emptyMakerChecker.motivation = "";
        emptyMakerChecker.files = [];
        emptyMakerChecker.model = JSON.stringify(user);

        this.http.post(`${this.makerCheckerUrl}/Register`, emptyMakerChecker, httpOptions).subscribe(
            data => console.log(data),
            error  => alert(error.error ?? error.message)
        );

        return { description: "testing", resultCode: 1 };
    }
    //#endregion
}
