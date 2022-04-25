import { Component, Output, EventEmitter } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalUpdateContentComponent } from "./../../views/user/user-update-popup.component";

// pop up imports moved to sidebar
// import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
// import { NgbdModalIndustryTypeContentComponent } from "./../../views/industry-type/industry-type-popup.component";
// import { NgbdModalVoucherTypeContentComponent } from "./../../views/voucher-type/voucher-type-popup.component";

@Component({
    selector: "app-header",
    templateUrl: "./header.component.html",
    styleUrls: ["./header.component.scss"]
})
export class AppHeaderComponent {

    // tslint:disable-next-line: prefer-output-readonly
    @Output() clickHamburgerEvent: EventEmitter<any> = new EventEmitter<any>();

    constructor(
        private readonly router: Router,
        private readonly modalService: NgbModal) {
        console.log("header constructor");
    }

    onUserClick() : void {
        // this.router.navigate(["/home/user"]);

        const modalRef = this.modalService.open(NgbdModalUpdateContentComponent);
        modalRef.componentInstance.user = {}; // TODO this needs to fetch current user
    }

    onNotificationClick() : void {
        this.router.navigate(["/home/notification"]);
    }

    onAlertsClick() : void {
        this.router.navigate(["/home/alerts"]);
    }

    onEntitlementClick() : void {
        this.router.navigate(["/home/entitlement"]);
    }

    // onMakerCheckerClick() : void {
    //     this.router.navigate(["/home/maker-checker"]);
    // }

    // onIndustryClick() : void {
    //     this.modalService.open(NgbdModalIndustryTypeContentComponent);
    // }

    // onVoucherClick() : void {
    //     this.modalService.open(NgbdModalVoucherTypeContentComponent);
    // }

    onHamburgerClick() {
        this.clickHamburgerEvent.emit();
    }
}

// TODO Header component requires a logo // I think this will be done through HTML only but not certain
