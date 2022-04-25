import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators, FormGroup, FormControl } from "@angular/forms";
import { AppUserService } from "./../../services/user.service";
import { MyErrorStateMatcher } from "./../../components/core/error-state-matcher";
import { Router } from "@angular/router";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html"
})

// Class to register a new user
export class RegisterComponent implements OnInit {
  form: FormGroup;
  emailRegex = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$";
  strongPassword = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$";


  matcher = new MyErrorStateMatcher();

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly userService: AppUserService,
    private readonly router: Router) {
      console.log("Register Component Constructor");

      this.form = this.formBuilder.group({
        firstName: [""],
        lastName: [""],
        email: [""], // email regex
        password: [""], // Strong password regex
        confirmPassword: [""]
      });

      // this.form.controls.password.valueChanges.subscribe(() => {
      //   this.form.controls.confirmPassword.updateValueAndValidity();
      // });
  }

  ngOnInit() {
    this.form = new FormGroup({
      firstName: new FormControl("", [Validators.required, Validators.minLength(3)]),
      lastName: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email, Validators.pattern(this.emailRegex)]),
      password: new FormControl("", [Validators.required, Validators.pattern(this.strongPassword)]),
      confirmPassword: new FormControl("", [Validators.required, Validators.pattern(this.strongPassword)])
    })
  }

  get password() { return this.form.get("password"); }
  get firstName() { return this.form.get("firstName"); }
  get lastName() { return this.form.get("lastName"); }
  get email() { return this.form.get("email"); }
  get confirmPassword() { return this.form.get("password"); }

  test() {
    console.log(this.form);
  }

  checkPasswords(form: FormGroup) : any {
    // the following algorithmm looks convoluted to adhere to strict null checking
    // yes there are better ways but those do not adhere to strict null checking

    let passCheck = "notsame";
    let confirmPassCheck = "not same as passcheck";

    console.log(passCheck);
    console.log(confirmPassCheck);

    const pass = form.get("password")
    if (!pass) {
      return { notSame: true };
    }
    passCheck = pass.value ?? "";

    const confirmPass = form.get("confirmPassword");
    if (!confirmPass) {
      return { notSame: true };
    }
    confirmPassCheck = confirmPass.value ?? "";

    console.log(pass === confirmPass ? null : { notSame: true });

    // return passCheck === confirmPassCheck ?? false
    // return { notSame: false };
    return passCheck === confirmPassCheck ? null : { notSame: true }
  }

  register() : void {
    // send a registration request with the entered credentials
    console.log("register() called within register component");


    if (this.form.invalid) {
      alert("Please ensure all fields are valid");
      return;
    }

    if (((this.checkPasswords(this.form) ?? { notsame: false }).notSame ?? false) === true) {
      alert("Passwords do not match"); // TODO this should be part of form validation
      return;
    }


    this.userService.register(this.form.value);

    alert("You will be notified via email once your account is accepted");

    this.router.navigate(["/signin"]);
  }
}


