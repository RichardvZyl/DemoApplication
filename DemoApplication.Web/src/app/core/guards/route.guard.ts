import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AppTokenService } from "../services/token.service";

@Injectable({ providedIn: "root" })
export class AppRouteGuard implements CanActivate {
    constructor(
        private readonly router: Router,
        private readonly appTokenService: AppTokenService) { }

    canActivate() {
        if (this.appTokenService.any()) { return true; }
        this.router.navigate(["/login"]);
        return false;
    }
}
