import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { AppAuthService } from "../../services/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: "app-signin",
    templateUrl: "./signin.component.html"
})

// Class for Sign in and route to other functions that can be performed without logging in
export class AppSignInComponent {

    form = this.formBuilder.group({
        login: ["", Validators.required],
        password: ["", Validators.required]
    });

    constructor(
        private readonly formBuilder: FormBuilder,
        private readonly appAuthService: AppAuthService,
        private readonly router: Router) {
            console.log("SignIn Constructor called");
    }

    signIn() {
        console.log("signIn() called within signIn component");

        this.appAuthService.signIn(this.form.value);
    }

    forgotPassword() {
        console.log("forgotPassword() called within signIn component");

        this.router.navigate(["/forgotPassword"]);
    }

    register() {
        console.log("register() called within signIn component");

        this.router.navigate(["/register"]);
    }
}
