import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { UserModel } from "../../models/user/user.model";

@Component({
  selector: "app-ngbd-modal-content",
  templateUrl: "user-popup.component.html"
})

export class NgbdModalUserContentComponent {
  @Input() user: UserModel = new UserModel();

  constructor(public activeModal: NgbActiveModal) {
    console.log("user popup constructor");
  }
}