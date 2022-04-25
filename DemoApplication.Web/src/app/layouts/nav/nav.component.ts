import { Component } from "@angular/core";
import { AppAuthService } from "./../../services/auth.service";
// import { Router } from "@angular/router";

// import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
    selector: "app-nav",
    templateUrl: "./nav.component.html",
    styleUrls: ["./nav.component.scss"]
 })

export class AppNavComponent {
    constructor(
        private readonly appAuthService: AppAuthService,
        // private readonly router: Router,
        // private readonly modalService: NgbModal
        ) {
              console.log("Nav component constructor");
        }

    signOut() {
        this.appAuthService.signOut();
    }

    // navToNotifications() {
    //     this.router.navigate(["/home/notification"])
    // }

    // navToUsers() {
    //     this.router.navigate(["/home/user"])
    // }

    // navToMakerChecker() {
    //     this.router.navigate(["/home/maker-checker"])
    // }

}