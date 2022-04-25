import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { UserModel } from "../../models/user/user.model";
import { AppUserService } from "../../services/user.service";
import { compare } from "fast-json-patch";
// import { Router } from "@angular/router";
// import { AppTokenService } from "../../core/services/token.service";

@Component({
  selector: "app-ngbd-modal-update-content",
  templateUrl: "user-update-popup.component.html"
})

// TODO this is working but input is very hacky // needs more work
// could not get forms to work
export class NgbdModalUpdateContentComponent implements OnInit {
  user: UserModel = new UserModel();

  // TODO still require service import to send new values to service // but first figure out why form is not working
  constructor(
    public activeModal: NgbActiveModal,
    private readonly userService: AppUserService,
    // private readonly router: Router,
    // private readonly appTokenService: AppTokenService
  ) {
    console.log("update user popup constructor");
  }

  ngOnInit() {
    this.userService.getCurrentUser().then(result => {
      this.user = result;
    });
  }


  updateUser(name: string, surname: string, email: string) {
    console.log("update User save button pressed");

    if (!email.toLowerCase().match("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")){
      alert("Please choose a valid email address");
      return;
    };

    console.log(email + surname + name)

    const updatedUser = new UserModel();
    updatedUser.active = this.user.active;
    updatedUser.auth = this.user.auth
    updatedUser.email = email;
    updatedUser.id = this.user.id;
    updatedUser.name = name;
    updatedUser.surname = surname;
    updatedUser.auth = this.user.auth;

    const patch = compare(this.user, updatedUser);

    console.log(patch);

    const patchDoc = JSON.stringify(patch);

    this.userService.updateUser(patchDoc, updatedUser);

    // this.appTokenService.clear();
    // this.router.navigate(["/signin"]);
  }
}