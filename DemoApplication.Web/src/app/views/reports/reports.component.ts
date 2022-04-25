// angular imports
import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";

// Model imports
import { ReportRequest } from "./../../models/reports/report-request.model"
import { ResultModel } from "./../../models/auth/result.model";
import { IndustryTypeResponseModel } from "./../../models/types/industry-type-repsonse.model";
import { VoucherTypeResponseModel } from "./../../models/types/voucher-types-response.model";
import { IndustryTypeRequestModel } from "../../models/types/industry-types-request.model";
import { VoucherTypeRequestModel } from "../../models/types/voucher-types-request.model";
// import { FilterModel } from "./../../models/reports/filters.model";

// Serivce imports
import { ReportsService } from "./../../services/reports.service";
import { TypesService } from "./../../services/types.service";
import { AppUserService } from "./../../services/user.service";
import { IndustryTypeModel } from "../../models/types/industry-type.model";
import { VoucherTypeModel } from "../../models/types/voucher-type.model";


@Component({
  selector: "app-reports",
  templateUrl: "./reports.component.html"
})

// class to contain report requests and filters
export class ReportsComponent implements OnInit {
  reportRequest: ReportRequest = new ReportRequest();
  resultModel: ResultModel = new ResultModel();
  industryTypeRequest: IndustryTypeRequestModel = new IndustryTypeRequestModel();
  voucherTypeRequest: VoucherTypeRequestModel = new VoucherTypeRequestModel();
  industryTypeResponse: IndustryTypeResponseModel = new IndustryTypeResponseModel();
  voucherTypeResponse: VoucherTypeResponseModel = new VoucherTypeResponseModel();

  reports: string[] = ["Audit Report", "Current Financial Report", "History Financial Report", "Statistical Report"];
  industryTypes: IndustryTypeModel[] = [];
  industryTypesString: string[] = [];
  voucherTypes: VoucherTypeModel[] = [];
  voucherTypesString: string[] = [];
  dateToday: string = Date.now().toString();
  users: string[] = [];
  formats: string[] = [];

  selectedReport: any;
  selectedIndustryType: any;
  selectedVoucherType: any;
  selectedUsers: any;
  picker: any;
  toDate: any;
  fromDate: any;

  form = this.formBuilder.group({
    report: ["Audit Report", Validators.required],
    fromDate: [this.dateToday],
    toDate: [this.dateToday],
    industryType: [""],
    voucherType: [""],
    users: [""],
    format: [""]
  }
  );

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly reportService: ReportsService,
    private readonly typeService: TypesService,
    private readonly userService: AppUserService) {
      console.log("Reports component constructor");
  }

  ngOnInit() {
    console.log("OnInit called within reports component");
    this.populateIndustryDropDown();
    this.populateVoucherDropDown();
    this.populateUserDropDown();
  }

  populateUserDropDown() {
    this.userService.getUsers().then(result => {
      const usersComplete = result;

      usersComplete.forEach(user => {
        this.users.push(user.email ?? "");
      })
    });
  }

  populateIndustryDropDown() {
    // populate dropdown menu with industry types
    console.log("populate IndustryDropDown called within reports component");

    this.industryTypeRequest = { session: { originator: "tester", requestNumber: 6 } };
    this.typeService.getIndustryTypes(this.industryTypeRequest).then(result => {
      this.industryTypeResponse = result;

      // TODO handle the response here
      if (!this.industryTypeResponse) {
      console.log("An error occured as an empty response was received from TypeService for IndsutryType");
    }
      console.log(this.industryTypeResponse);

      ((this.industryTypeResponse ?? { industryTypes: [] }).industryTypes ?? []).forEach(industryType => {
        this.industryTypesString?.push(industryType.id + " " + industryType.shortDescription);
      });

      this.industryTypes = this.industryTypeResponse.industryTypes ?? [];
    });
  }

  populateVoucherDropDown() {
    console.log("Populate voucher type dropdown");
    this.voucherTypeRequest = { session: { originator: "tester", requestNumber: 6 } };
    this.typeService.getVoucherTypes(this.voucherTypeRequest).then(result => {
      this.voucherTypeResponse = result;

    // TODO handle the response here
      if (!this.industryTypeResponse) {
      console.log("An error occured as an empty repsonse was received from typeService for Vouchertypes");
    }
      console.log(this.voucherTypeResponse);

      ((this.voucherTypeResponse ?? { voucherTypes: [] }).voucherTypes ?? []).forEach(voucherType => {
        this.voucherTypesString?.push(voucherType.id + " " + voucherType.shortDescription);
      });

      this.voucherTypes = this.voucherTypeResponse.voucherTypes ?? [];
    });
  }

  requestReport() : ResultModel {
    console.log("requestReport() Called within reports component");

    // const formValues = this.form.value;
    const reportRequest: ReportRequest = {
       report: this.selectedReport ?? "",
       toDate: this.fromDate ?? new Date(),
       fromDate: this.toDate ?? new Date(),
       voucherType: this.selectedVoucherType ?? "",
       industryType: this.selectedIndustryType ?? "",
       users: this.selectedUsers ?? []
    };
    console.log(reportRequest);

    this.resultModel = this.reportService.requestReport(reportRequest);

    // TODO handle result model here //below is only for testing purposes //build in a real log
    if (this.resultModel.resultCode === 0 || !this.resultModel) {
      // is 0 code used for errors?
      console.log(this.resultModel.description);
    }

    return this.resultModel;
  }

}
