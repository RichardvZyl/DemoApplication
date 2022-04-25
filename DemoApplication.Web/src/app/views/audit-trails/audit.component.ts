// angular imports
import { Component, OnInit, ViewChild } from "@angular/core";
import { Sort, MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";

// model imports
// import { ResultModel } from "./../../models/auth/result.model";
import { AccountingLogModel } from "../../models/accounting/accounting.model";
import { AuditLogModel } from "../../models/audit/audit.model";

// service imports
import { AppAuditService } from "../../services/audit.service";

// pop up imports
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalAuditContentComponent } from "./audit-popup.component";


@Component({
  selector: "app-accounting",
  templateUrl: "./audit.component.html"
})

// Industry Types class and add IndustryType
// black table
export class AuditComponent implements OnInit {
  displayedColumns: string[] = ["date", "user", "displayContext", "action"];
  auditEntries: AuditLogModel[] = []; // used to output to table // ignores other response parameters
  dataSource: MatTableDataSource<AuditLogModel> = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private readonly auditService: AppAuditService,
    private readonly modalService: NgbModal
    ) {
    console.log("Accounting Constructor");
  }

  ngOnInit(): void {
    // build request to send to service
    console.log("Accounting Log component initialize called");

    this.auditService.getAuditLogs().then(result => {
      this.auditEntries = result;

      // TODO look at the response and act accordingly
      if (!this.auditEntries) {
        console.log("Error Occured as no accounting entries was received");
        return;
      }
      console.log(this.auditEntries);

      this.auditEntries = this.auditEntries.slice();
      this.dataSource = new MatTableDataSource(this.auditEntries);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  open(account: AccountingLogModel) {
    const modalRef = this.modalService.open(NgbdModalAuditContentComponent);
    modalRef.componentInstance.auditEntry = account;
  }

  applyFilter(filterValue: Event) : void {
    // filter table results based on input string
    let filterString = (filterValue.target as HTMLInputElement).value ?? "";
    filterString = filterString.trim(); // Remove whitespace
    filterString = filterString.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterString;
  }

  sortData(sort: Sort) {
    const data = this.auditEntries.slice();
    if (!sort.active || sort.direction === "") {
      return;
    }

    this.auditEntries = data.sort((a, b) => {
      const isAsc = sort.direction === "asc";
      switch (sort.active) {
        case "id": return compare(a.id ?? "", b.id ?? "", isAsc);
        case "date": return compare(a.date ?? "", b.date ?? "", isAsc);
        case "userId": return compare(a.userId ?? "", b.userId ?? "", isAsc);
        case "displayContext": return compare(a.displayContext ?? "", b.displayContext ?? "", isAsc);
        case "model": return compare(a.model ?? "", b.model ?? "", isAsc);
        case "contents": return compare(a.contents ?? "", b.contents ?? "", isAsc);
        default: return 0;
      }
    });
  }

}

function compare(a: number | string | Date, b: number | string | Date, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
