import { Injectable } from "@angular/core";
import { NotificationModel } from "../models/notifications/notification.model";
import { NotificationRequestModel } from "../models/notifications/notification-request.model";
import { NotificationResponseModel } from "../models/notifications/notification-response.model";
import { Observable, EMPTY } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
// import { HttpClient } from "@angular/common/http";


const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json"
  })
}

@Injectable({
  providedIn: "root"
})

// This class handels backend interaction for notifications and alerts
export class NotificationService {
  notificationsUrl = environment.apiUrl + "Notifications";
  alertsUrl = environment.apiUrl + "Notifications/Alerts";
  angularListUrl = environment.apiUrl + "AngularList";

  notifications: NotificationModel[] = [];
  notficationResponse: NotificationResponseModel = new NotificationResponseModel();

  constructor(private readonly http: HttpClient) {
    console.log("NotificationService log");
  }

  async getAlerts(notificationRequest: NotificationRequestModel) : Promise<NotificationResponseModel> {
    // Requests all notifications
    console.log("getAlerts function called within NotificationService");

    if (!notificationRequest) {
      console.log("An error occured within getAlerts as an empty notificationRequest was received");
      return new NotificationResponseModel();
    }
    console.log(notificationRequest);
    // return this.http.post<string>(this.url, notificationRequest, httpOptions);

    this.notifications = await this.http.get<NotificationModel[]>(`${this.angularListUrl}/Notifications/Alerts`).toPromise();

    if (!this.notifications) {
      console.log("No notifications received from API");
      return new NotificationResponseModel();
    }
    console.log(this.notifications);

    return { // for testing
      session: { originator: "Richard", requestNumber: 1 },
      result: { resultCode: 1, description: "testing" },
      notifications: this.notifications
    };
  }

  async getNotifications(notificationRequest: NotificationRequestModel) : Promise<NotificationResponseModel> {
    // Requests all notifications
    console.log("getNotifications function called within NotificationService");

    if (!notificationRequest) {
      console.log("An error occured within getNotifications as an empty notificationRequest was received");
      return new NotificationResponseModel();
    }
    console.log(notificationRequest);
    // return this.http.post<string>(this.url, notificationRequest, httpOptions);

    this.notifications = await this.http.get<NotificationModel[]>(`${this.angularListUrl}/Notifications`).toPromise();

    if (!this.notifications) {
      console.log("No notifications received from API");
      return new NotificationResponseModel();
    }
    console.log(this.notifications);

    return { // for testing
      session: { originator: "Richard", requestNumber: 1 },
      result: { resultCode: 1, description: "testing" },
      notifications: this.notifications
    };
  }

  getNotificationDetails(notification: NotificationModel) : Observable<NotificationModel> {
    // Show detail of the selected notification //(expand)
    console.log("getNotificationDetails called within notificationService");

    if (!notification) {
      console.log("An error occured within getNotificationDetails as an empty notification was received");
      return EMPTY;
    }
    console.log(notification);

    return this.http.get<NotificationModel>(`Notifications/${notification.id}`);
  }

  readNotification(notification: NotificationModel) {
    console.log("ReadNotifications called within notification service");

    if (!notification) {
      console.log("An error occured within readNotifications as an empty notification was received");
      return;
    }
    console.log(notification);

    this.http.post(`${this.notificationsUrl}/${notification.id}`, "", httpOptions).subscribe(
      data => console.log(data),
      error  => alert(error.error ?? error.message)
    );
  }


}