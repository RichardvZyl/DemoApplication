import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { AuditLogModel } from "../../models/audit/audit.model";


@Component({
  selector: "app-ngbd-modal-accounting-content",
  templateUrl: "audit-popup.component.html"
})

export class NgbdModalAuditContentComponent {
  @Input() auditEntry: AuditLogModel = new AuditLogModel();

  constructor(
    public activeModal: NgbActiveModal
    ) {
    console.log("Audit log pop up constructor");
  }

}