// angular imports
import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";

// model imports
import { UserModel } from "./../../models/user/user.model";
import { SuspendUserRequestModel } from "./../../models/user/suspend-user-request.model";

// service imports
import { AppUserService } from "./../../services/user.service";

// pop up imports
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalUserContentComponent } from "./user-popup.component";
import { NgbdModalUpdateContentComponent } from "./user-update-popup.component";
import { UserModelNoAuth } from "../../models/user/user-noauth.model";
import { NewMakerCheckerModel } from "../../models/auth/new-maker-checker.model";
import { NgbdModalMotivationContentComponent } from "../maker-checker/motivation-popup.component";


@Component({
  selector: "app-user",
  templateUrl: "./user.component.html"
})

// class used to display users and interactions with them
// black table
export class UserComponent  implements OnInit {
  displayedColumns: string[] = ["name", "surname", "email", "active", "action"];
  users: UserModelNoAuth[] = [];
  dataSource: MatTableDataSource<UserModel> = new MatTableDataSource();
  suspendRequest: SuspendUserRequestModel = new SuspendUserRequestModel();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private readonly userService: AppUserService,
    private readonly router: Router,
    private readonly modalService: NgbModal) {
      console.log("Users constructor");
    }

  ngOnInit(): void {
    // get a list of users
    console.log("onInit() called within Users component");

    this.userService.getUsers().then(result => {
      this.users = result;

      if (!this.users) {
        console.log("An error occured as an empty model or no response was received from userService");
        return;
      }
      console.log(this.users);

      this.users = this.users.slice() ?? [];
      this.dataSource = new MatTableDataSource(this.users);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(filterValue: Event) : void {
    // filter table results based on input string
    let filterString = (filterValue.target as HTMLInputElement).value ?? "";
    filterString = filterString.trim(); // Remove whitespace
    filterString = filterString.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterString;
  }

  open(user: UserModel) : void {
    // view selected user
    console.log("onView() called within user component");

    if (!user) {
      console.log("An error occured as an empty UserModel was received from onView()");
      return;
    }

    // return this.userService.getUser(user);

    const modalRef = this.modalService.open(NgbdModalUserContentComponent);
    modalRef.componentInstance.user = user;
  }

  onSuspend(userInput: UserModel) : void {
    // suspend user request
    console.log("onSuspend() called within user component");

    if (!userInput) {
      console.log("An error occured within user component as an empty userModel was received");
      return;
    }
    console.log(userInput);

    const modalRef = this.modalService.open(NgbdModalMotivationContentComponent);
    modalRef.componentInstance.input = userInput.id;
    modalRef.result.then((motivationResult: NewMakerCheckerModel) => {
      if (motivationResult) {
        console.log(motivationResult);
        this.userService.suspendUser(motivationResult);
      }
    });
  }

  onUpdate(user: UserModel) : void {
    // reroute to the update user component
    console.log("onUpdate() called within user component");

    if (!user) {
      console.log("An error occured within user component as an empty userModel was received");
      return;
    }
    console.log(user);

    const modalRef = this.modalService.open(NgbdModalUpdateContentComponent);
    modalRef.componentInstance.user = user;
  }

  isHome() : boolean {
    return this.router.url === "/home";
  }
}
