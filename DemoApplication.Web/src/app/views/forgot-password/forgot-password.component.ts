import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
// import { AppAuthService } from "src/app/services/auth.service";
import { AppUserService } from "./../../services/user.service";

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html"
})

// Component for ForgotPassword screen
// provides the ability to request a password reset link via email
export class ForgotPasswordComponent {

  form = this.formBuilder.group({
    email: ["", [Validators.required, Validators.email]]
  });

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly userService: AppUserService) {
      console.log("Forgot Password Constructor");
  }

  forgotPassword() : void {
    // send a password reset request
    console.log("forgotPassword called within forgotPassword component");

    this.userService.resetPassword(this.form.value);
  }

}

