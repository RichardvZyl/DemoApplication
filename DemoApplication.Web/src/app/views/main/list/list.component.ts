import { Component, OnDestroy } from "@angular/core";
import { AppUserService } from "./../../../services/user.service";
import { UserModel } from "./../../../models/user/user.model";
import { Subscription } from "rxjs";

@Component({ selector: "app-list", templateUrl: "./list.component.html" })
export class AppListComponent implements OnDestroy {
    private listSubscription: Subscription;

    users = new Array<UserModel>();

    constructor(private readonly appUserService: AppUserService) {
        this.listSubscription = this.appUserService.list().subscribe((users) => this.users = users);
    }

    ngOnDestroy(): void {
        this.listSubscription.unsubscribe();
    }
}
