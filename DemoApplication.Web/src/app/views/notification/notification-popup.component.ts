import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { NotificationModel } from "../../models/notifications/notification.model";
import { SeverityEnum, EntityEnum, RolesEnum } from "../../models/enums.model";

@Component({
  selector: "app-ngbd-modal-content",
  templateUrl: "notification-popup.component.html"
})

export class NgbdModalNotificationContentComponent {
  @Input() notification: NotificationModel = {};
  entity = EntityEnum;
  severity = SeverityEnum;
  role = RolesEnum;

  constructor(public activeModal: NgbActiveModal) {
    console.log("Modal popUp constructor");
  }
}