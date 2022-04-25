import { Injectable } from "@angular/core";
import { ReportRequest } from "./../models/reports/report-request.model";
import { ResultModel } from "./../models/auth/result.model";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json"
  })
}

@Injectable({
  providedIn: "root"
})

// Class is used to interact with backend pertaining to reports
export class ReportsService {
  reportsUrl = environment.apiUrl + "Reports";

  constructor(private http: HttpClient) {
    console.log("Report Service Constructor");
  }

  requestReport(reportRequest: ReportRequest) : ResultModel {
    // intent is to use just this class for all reports and pass the type of report requested in the ReportRequestModel
    console.log("requestReport called within reportsService");

    if (!reportRequest) {
        console.log("An Error Occured as an empty report request was received");
        return new ResultModel();
    }
    console.log(reportRequest);

    this.http.post(this.reportsUrl, reportRequest, httpOptions).subscribe(
      data => console.log(data),
      error  => alert(error.error ?? error.message)
    );

    return { description: "testing", resultCode: 5 };
  }

}