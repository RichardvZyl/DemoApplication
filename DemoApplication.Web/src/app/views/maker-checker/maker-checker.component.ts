// angular imports
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";

// model imports
import { MakerCheckerModel } from "../../models/auth/maker-checker.model";
import { MakerCheckerActionsEnum } from "../../models/enums.model";

// service imports
import { AppAuthService } from "./../../services/auth.service"

// pop up imports
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalMakerCheckerContentComponent } from "./maker-checker-popup.component";


@Component({
  selector: "app-maker-checker",
  templateUrl: "./maker-checker.component.html"
})
export class MakerCheckerComponent implements OnInit {
  displayedColumns: string[] = ["action", "context", "makerUser", "checkerUser", "accepted", "makerDate", "checkerDate", "view"];
  makerCheckers: MakerCheckerModel[] = [];
  dataSource: MatTableDataSource<MakerCheckerModel> = new MatTableDataSource();
  makerCheckerActions = MakerCheckerActionsEnum;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private readonly authService: AppAuthService,
    private readonly modalService: NgbModal) {
      console.log("Users constructor");
    }

  ngOnInit(): void {
    // get a list of users
    console.log("onInit() called within makerChecker component");

    this.authService.getMakerChecker().then(result => {
      this.makerCheckers = result;

      if (!this.makerCheckers) {
        console.log("An error occured as an empty model or no response was received from authService");
        return;
      }
      console.log(this.makerCheckers);

      this.makerCheckers = this.makerCheckers.slice() ?? [];
      this.dataSource = new MatTableDataSource(this.makerCheckers);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(filterValue: Event) : void {
    // filter table output to entered string
    let filterString = (filterValue.target as HTMLInputElement).value ?? "";
    filterString = filterString.trim(); // Remove whitespace
    filterString = filterString.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterString;
  }

  open(makerChecker: MakerCheckerModel) : void {
    // view selected user
    console.log("onView() called within user component");

    if (!makerChecker) {
      console.log("An error occured as an empty makerChecker was received from onView()");
      return;
    }

    // return this.userService.getUser(user);

    const modalRef = this.modalService.open(NgbdModalMakerCheckerContentComponent);
    modalRef.componentInstance.makerChecker = makerChecker;
  }
}