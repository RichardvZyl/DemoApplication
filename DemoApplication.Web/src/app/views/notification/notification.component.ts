// angular imports
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort, Sort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Router } from "@angular/router";

// Model imports
import { NotificationModel } from "./../../models/notifications/notification.model";
import { NotificationRequestModel } from "./../../models/notifications/notification-request.model";
import { NotificationResponseModel } from "./../../models/notifications/notification-response.model";
import { SeverityEnum, EntityEnum, RolesEnum } from "../../models/enums.model";

// Service import
import { NotificationService } from "./../../services/notification.service";

// pop up imports
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalNotificationContentComponent } from "./notification-popup.component";


@Component({
  selector: "app-notification",
  templateUrl: "./notification.component.html"
})

// class for notifications and interactions with specific notification
// Black table
export class NotificationComponent implements OnInit {
  notificationRequest: NotificationRequestModel = new NotificationRequestModel(); // request sent to NotificationService
  notificationResponse: NotificationResponseModel = new NotificationResponseModel(); // response received from NotificationService

  notifications: NotificationModel[] = []; // notification Model housed within NotificationResponse contains the actual notification
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  displayedColumns: string[] = ["originatorString", "severity", "description", "date", "entity", "seenByUser", "relatedDescription", "action"];

  entity = EntityEnum;
  severity = SeverityEnum;
  role = RolesEnum;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  constructor(private readonly notificationService: NotificationService,
              private readonly modalService: NgbModal,
              private readonly router: Router) {
    console.log("Notification Constructor");
  }

  ngOnInit(): void {
    // on screen load fetch notifications to display
    console.log("Notifications component initialize called");

    // build request to send to service // TODO should this occur in the service?
    this.notificationRequest = {
      session: { originator: "Richard", requestNumber: 5 }
    };

    // get alerts or notifications
    if (this.isAlerts()) {
      this.notificationService.getAlerts(this.notificationRequest).then(result => {
        this.notificationResponse = result;

        // TODO look at the response and act accordingly
        // Test case ignores response and only uses the notifications received
        if (!this.notificationResponse) {
          console.log("Error Occured as no notificationResponse was received");
          return;
        }
        console.log(this.notificationResponse);

        // discard reponse and use only notifications
        this.notifications = this.notificationResponse.notifications ?? [];
        console.log(this.notifications);

        this.notifications = this.notifications.slice() ?? [];
        this.dataSource = new MatTableDataSource(this.notifications);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
    } else {
      this.notificationService.getNotifications(this.notificationRequest).then(result => {
        this.notificationResponse = result;

        // TODO look at the response and act accordingly
        // Test case ignores response and only uses the notifications received
        if (!this.notificationResponse) {
          console.log("Error Occured as no notificationResponse was received");
          return;
        }
        console.log(this.notificationResponse);

        // discard reponse and use only notifications
        this.notifications = this.notificationResponse.notifications ?? [];
        console.log(this.notifications);

        this.notifications = this.notifications.slice() ?? [];
        this.dataSource = new MatTableDataSource(this.notifications);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
    }
  }

  applyFilter(filterValue: Event) : void {
    // filter table output based on enetered string
    console.log(filterValue);

    let filterString = (filterValue.target as HTMLInputElement).value ?? "";
    filterString = filterString.trim(); // Remove whitespace
    filterString = filterString.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterString;
  }

  read(notification: NotificationModel) : void {
    console.log("read notification called within notification component");

    if (!notification) {
      console.log("An error occured as notification is empty");
    }
    console.log(notification);

    this.notificationService.readNotification(notification);
  }

  open(notificationAsDisplayed: NotificationModel) : void {
    // expand selected notification
    console.log("onView called within notification component");

    if (!notificationAsDisplayed) {
      console.log("An error occured as an empty notificationModel was received within onView");
      return;
    }
    console.log(notificationAsDisplayed);

    // rework Inotification into NotificationModel // not needed as modal expects Inotification
    // const notificationString = notificationAsDisplayed.toString() ?? "";
    // const indexer = notificationString.indexOf(" ") ?? 0;
    // const severityCode = Number(notificationString.slice(0, indexer) ?? 0) ? NaN : 0;
    // const severityDescriptionString = notificationString.slice(indexer + 1) ?? "";

    // const notificationReworked: NotificationModel = {
    //   originator: notificationAsDisplayed.originator ?? "",
    //   severity: { code: severityCode, description: severityDescriptionString },
    //   description: notificationAsDisplayed.description ?? ""
    // }

    const modalRef = this.modalService.open(NgbdModalNotificationContentComponent);
    modalRef.componentInstance.notification = notificationAsDisplayed;
  }

  isHome() : boolean {
    // used to hide filter and paginator on main screen
    return this.router.url === "/home";
  }

  isAlerts() : boolean {
    return this.router.url === "/home/alerts";
  }

  sortData(sort: Sort) {
    const data = this.notifications.slice();
    if (!sort.active || sort.direction === "") {
      return;
    }

    this.notifications = data.sort((a, b) => {
      const isAsc = sort.direction === "asc";
      switch (sort.active) {
        case "id": return compare(a.id ?? "", b.id ?? "", isAsc);
        case "originator": return compare(a.originator ?? "", b.originator ?? "", isAsc);
        case "severity": return compare(a.severity ?? "", b.severity ?? "", isAsc);
        case "description": return compare(a.description ?? "", b.description ?? "", isAsc);
        case "date": return compare(a.date ?? "", b.date ?? "", isAsc);
        case "read": return compare(String(a.read) ?? "", String(b.read) ?? "", isAsc);
        case "seenBy": return compare(a.seenBy ?? "", b.seenBy ?? "", isAsc);
        case "seenAt": return compare(a.seenAt ?? "", b.seenAt ?? "", isAsc);
        case "forRole": return compare(a.forRole ?? "", b.forRole ?? "", isAsc);
        case "relatedId": return compare(a.relatedId ?? "", b.relatedId ?? "", isAsc);
        case "entity": return compare(a.entity ?? "", b.entity ?? "", isAsc);
        default: return 0;
      }
    });
  }
}

function compare(a: number | string | Date, b: number | string | Date, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
