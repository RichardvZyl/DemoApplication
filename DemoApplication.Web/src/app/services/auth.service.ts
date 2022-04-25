import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AppTokenService } from "../core/services/token.service";
import { SignInModel } from "../models/auth/signIn.model";
import { TokenModel } from "../models/auth/token.model";
import { RegisterModule } from "../views/register/register.module";
import { MakerCheckerModel } from "./../models/auth/maker-checker.model";
// import { MakerCheckerActionsEnum } from "./../models/enums.model";
import { ResultModel } from "../models/auth/result.model";

// import { EntitlementModel } from "../models/auth/entitlement.model";
import { NewMakerCheckerModel } from "../models/auth/new-maker-checker.model";
import { EntitilementExceptionsModel } from "../models/auth/entitlement-exceptions.model";
import { environment } from "../../environments/environment";
// import { RolesEnum } from "../models/enums.model";
// import { EntitilementExceptionsModel } from "../models/auth/entitlement-exceptions.model";
// import { MotivationModel } from "./../models/auth/motivation.model";

const httpOptions = {
    headers: new HttpHeaders({
      "Content-Type": "application/json"
    })
}

@Injectable({ providedIn: "root" })

// This class was included in the template and gives good examples on how to interact with the backend
export class AppAuthService {
    authUrl = environment.apiUrl + "Auths";
    entitlementUrl = environment.apiUrl + "Entitlement";
    makerCheckerUrl = environment.apiUrl + "MakerCheckers"
    angularListUrl = environment.apiUrl + "AngularList";

    constructor(
        private readonly http: HttpClient,
        private readonly router: Router,
        private readonly appTokenService: AppTokenService) {
            console.log("Auth Service Constructor");
        }

    signIn(model: SignInModel): void {
        // Sign in a user based on the signInModel
        console.log("signIn function called within authService");

        if (!model) {
            console.log("An Error occured as an empty signInModel was received within signIn function");
            return;
        }
        console.log(model);

        this.http
            .post<TokenModel>(`${this.authUrl}`, model)
            .subscribe((result) => {
                if (!result || !result.token) { return; }
                this.appTokenService.set(result.token);
                this.router.navigate(["/home"]);
            });
    }

    signOut() : void {
        // Sign out the current user
        console.log("signOut function called within authService");

        this.appTokenService.clear();
        this.router.navigate(["/login"]);
    }

    resetPassword(model: any) : void {
        // Request a password reset email // backend will handle the email service
        console.log("resetPassword function called within authService");

        if (!model) {
            console.log("An error occured within resetPassword function as an empty model was received");
            return;
        }
        console.log(model);

        this.http
            .post<TokenModel>(`${this.authUrl}`, model)
            .subscribe((result) => {
                if (!result || !result.token) { return; }
                this.appTokenService.set(result.token);
                this.router.navigate(["/login"]);
            });
    }

    register(model: RegisterModule) : void {
        // Request a new registration that will be handled in the backend
        console.log("register function called within authService");

        if (!model) {
            console.log("An error occured within register function as an empty model was received");
            return;
        }
        console.log(model);

        this.http
            .post<TokenModel>("Auths", model)
            .subscribe((result) => {
                if (!result || !result.token) { return; }
                this.appTokenService.set(result.token);
                this.router.navigate(["/login"]);
            });
    }

    async getMakerChecker() : Promise<MakerCheckerModel[]> {
        // get all maker checkers from backend
        console.log("getMakerCheckers called from authService");

        const makerCheckers: MakerCheckerModel[] = await this.http.get<MakerCheckerModel[]>(`${this.angularListUrl}/MakerCheckers`).toPromise();

        if (!makerCheckers) {
            console.log("An error occured as no maker checkers were received from the backend");
        }

        return makerCheckers;
    }

    actionMakerChecker(makerChecker: MakerCheckerModel, accept: boolean) : ResultModel {
        console.log("Save makeChecker called within Auth Service");

        if (!makerChecker) {
            console.log("An error occured as an empty maker checker has been received");
            return new ResultModel();
        }
        console.log(makerChecker);

        if (accept) {
            this.http.post(`${this.makerCheckerUrl}/Approve/${makerChecker.id}`, "", httpOptions).subscribe(
                data => console.log(data),
                error  => alert(error.error ?? error.message)
            );
        } else {
            this.http.post(`${this.makerCheckerUrl}/Deny/${makerChecker.id}`, "", httpOptions).subscribe(
                data => console.log(data),
                error  => alert(error.error ?? error.message)
            );
        }

        return new ResultModel();
    }


    saveUserEntitlement(entitlement: NewMakerCheckerModel) : ResultModel {
        console.log("saveUserEntitlement called within userService");

        if (!entitlement) {
            console.log("An error occured as an empty entitlement model was received within userService");
        }
        console.log(entitlement);

        this.http.post(`${this.makerCheckerUrl}/Entitlement`, entitlement, httpOptions).subscribe(
            data => console.log(data),
            error  => alert(error.error ?? error.message)
        );

        return { description: "testing", resultCode: 1 };
    }

    async getUserEntitlement(userEmail: string) : Promise<EntitilementExceptionsModel> {
        console.log("getUserEntitlement called within user service");
        // get users current entitlement
        const entitlement = await this.http.get<EntitilementExceptionsModel>(`${this.entitlementUrl}/User/${userEmail}`).toPromise();
        return entitlement;
    }

    async getUserRole(userEmail: string) : Promise<any> {
        console.log("getUserRole called within user service");
        const role = await this.http.get(`${this.entitlementUrl}/User/Role/${userEmail}`, {responseType: "text"}).toPromise();
        return role;
    }

    async getDefaultEntitlement(userType: number) : Promise<EntitilementExceptionsModel> {
        console.log("get dafault entitlement");

        const entitlement = await this.http.get<EntitilementExceptionsModel>(`${this.entitlementUrl}/Role/${userType}`).toPromise();
        return entitlement;
    }
}
