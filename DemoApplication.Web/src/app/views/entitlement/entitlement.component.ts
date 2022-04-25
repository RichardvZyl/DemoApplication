// Angular imports
import { Component, OnInit } from "@angular/core";
// import { FormBuilder } from "@angular/forms"; // Validators

// Model imports
import { EntitlementModel } from "./../../models/auth/entitlement.model";
import { EntitilementExceptionsModel } from "./../../models/auth/entitlement-exceptions.model";
import { NewMakerCheckerModel } from "../../models/auth/new-maker-checker.model";
// import { MotivationModel } from "../../models/auth/motivation.model";

// Service imports
import { AppUserService } from "./../../services/user.service";
import { AppAuthService } from "../../services/auth.service";

// pop up imports
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbdModalMotivationContentComponent } from "../maker-checker/motivation-popup.component";
import { FormBuilder } from "@angular/forms";


@Component({
  selector: "app-entitlement",
  templateUrl: "./entitlement.component.html"
})


export class EntitlementComponent implements OnInit {
  users: string[] = [];
  userTypes: string[] = [];
  selectedUserType: any;
  selectedUser = "";
  entitlement: EntitlementModel = new EntitlementModel();
  entitlementActionedExceptions: EntitilementExceptionsModel = new EntitilementExceptionsModel();
  entitlementExceptions: EntitilementExceptionsModel = new EntitilementExceptionsModel();

  form = this.formBuilder.group({
    user: "",
    role: ""
  });

  constructor(// private readonly modalService: NgbModal,
              private readonly formBuilder: FormBuilder,
              private readonly userService: AppUserService,
              private readonly authService: AppAuthService,
              private readonly modalService: NgbModal) {
    console.log("Entitlment constructor");
  }

  ngOnInit(): void {
    console.log("OnInit called within Entitlement Component");

    this.populateUserDropDown();
    this.populateUserTypesDropDown();
  }

  populateUserDropDown() : void {
    // populate the dropdown menu for users
    console.log("Populate User Drop Down");

    this.userService.getUsers().then(result => {
      const usersConst = result;

      usersConst.forEach(user => {this.users.push(user.email ?? "")});
    });
  }

  populateUserTypesDropDown() : void {
    console.log("Populate User Type DropDown");

    this.userService.getUserTypes().then(result => {
      this.userTypes = result;
    });
  }

  userTypeChanged(userType: string, event: any) : void {
    // get default entitlement based on selected user type
    if (event.isUserInput) { // ignore on deselection of the previous option
      console.log("User Type Changed");
      console.log(userType);

      const userTypeCode = userType.split(" ", 1); // get the enum int value of rolesEnum

      this.authService.getDefaultEntitlement(Number(userTypeCode[0])).then(result => {
       this.entitlementExceptions = result;
      });

      // this.entitlementActionedExceptions = new EntitilementExceptionsModel();
      this.exceptionActioned({ source: { id: "clear" } }); // clear exceptions
      this.savedExceptionActioned({ source: { id: "clear" } }, false);
    } else {
      // ignore event not actioned by user
    }
  }

  userChanged(userEmail: string, event: any) : void {
    // get selected user entitlement
    if (event.isUserInput) { // ignore on deselection of the previous option
      console.log("User Changed");
      console.log(userEmail);

      this.authService.getUserRole(userEmail).then(roleResult => {

        this.selectedUserType = roleResult;

        const userTypeCode = roleResult.split(" ", 1); // get the enum int value of rolesEnum

        this.authService.getDefaultEntitlement(Number(userTypeCode[0])).then(result => {
          this.entitlementExceptions = result;

          this.authService.getUserEntitlement(userEmail).then(entitilementResult => {
            this.applyExceptions(entitilementResult);
          });

         });

      });

      // this.entitlementActionedExceptions = new EntitilementExceptionsModel();
      this.exceptionActioned({ source: { id: "clear" } }); // clear exceptions
      this.savedExceptionActioned({ source: { id: "clear" } }, false);
    } else {
      // ignore event not actioned by user
    }
  }

  applyExceptions(entilement: EntitilementExceptionsModel) {

    const array = this.generateArray(entilement);
    array.forEach(element => {
      if (element.type !== "userId" && element.type !== "expiresOn") {
        console.log(element);
        if (element.value !== null) {
          this.savedExceptionActioned({ source: { id: element.type } }, element.value);
          this.exceptionActioned({ source: { id: element.type } }, element.value);
        }
      }
    });
  }

  generateArray(obj: any){
    return Object.keys(obj).map(key => ({type: key, value: obj[key]}));
 }

  saveUserEntitlement() : void {
    // save user entitlement
    if (!this.selectedUser) {
      alert("You need to select a user");
      return;
    }
    if (!this.selectedUserType) {
      alert("You need to select a role");
      return;
    }

    console.log("Save User called within entitlement component");
    console.log(this.selectedUserType);

    const userRoleArray = this.selectedUserType.split(" ");
    const userRole = userRoleArray[0];

    this.entitlement.login = this.selectedUser;
    this.entitlement.role = userRole;
    this.entitlement.claimExceptions = this.entitlementActionedExceptions ?? new EntitilementExceptionsModel();
    console.log(this.entitlement);

    const modalRef = this.modalService.open(NgbdModalMotivationContentComponent);
    modalRef.componentInstance.input = this.entitlement;
    modalRef.result.then((motivationResult: NewMakerCheckerModel) => {
      if (motivationResult) {
        console.log(motivationResult);
        this.authService.saveUserEntitlement(motivationResult);
        this.selectedUser = "";
        this.selectedUserType = "";
        this.exceptionActioned({ source: { id: "clear" } });
        this.savedExceptionActioned({ source: { id: "clear" } }, false);
      }
    });
  }

  exceptionActioned(values: any, value: boolean = false){
      switch (((values ?? { source: { id: "" }}).source ?? {id: ""}).id ?? "") {
        case "viewNotifications":
          this.entitlementActionedExceptions.viewNotifications = values.source.checked ?? value;
          break;

        case "viewUsers":
          this.entitlementActionedExceptions.viewUsers = values.source.checked ?? value;
          break;

        case "viewVouchers":
          this.entitlementActionedExceptions.viewVouchers = values.source.checked ?? value;
          break;

        case "suspendUsers":
          this.entitlementActionedExceptions.suspendUsers = values.source.checked ?? value;
          break;

        case "refundVouchers":
          this.entitlementActionedExceptions.refundVouchers = values.source.checked ?? value;
          break;

        case "addVoucherTypes":
          this.entitlementActionedExceptions.addVoucherTypes = values.source.checked ?? value;
          break;

        case "addIndustryTypes":
          this.entitlementActionedExceptions.addIndustryTypes = values.source.checked ?? value;
          break;

        case "statisticalReport":
          this.entitlementActionedExceptions.statisticalReport = values.source.checked ?? value;
          break;

        case "auditReport":
          this.entitlementActionedExceptions.auditReport = values.source.checked ?? value;
          break;

        case "financialReport":
          this.entitlementActionedExceptions.financialReport = values.source.checked ?? value;
          break;

        case "authorizeMakerChecker":
          this.entitlementActionedExceptions.authorizeMakerChecker = values.source.checked ?? value;
          break;

        case "entitlementChange":
          this.entitlementActionedExceptions.entitlementChange = values.source.checked ?? value;
          break;

        case "transferVouchers":
          this.entitlementActionedExceptions.transferVouchers = values.source.checked ?? value;
          break;

        case "auditLogs":
          this.entitlementActionedExceptions.auditLogs = values.source.checked ?? value;
          break;

        case "accountingLogs":
          this.entitlementActionedExceptions.accountingLogs = values.source.checked ?? value;
          break;

        case "viewMerchants":
          this.entitlementActionedExceptions.viewMerchants = values.source.checked ?? value;
          break;

        case "addMerchants":
          this.entitlementActionedExceptions.addMerchants = values.source.checked ?? value;
          break;

        case "updateMerchants":
          this.entitlementActionedExceptions.updateMerchants = values.source.checked ?? value;
          break;

        case "clear":
          this.entitlementActionedExceptions = new EntitilementExceptionsModel();
          break;

        default: return; // ignore other things that trigger this
      };
  }

  savedExceptionActioned(values: any, value: boolean){
    switch (((values ?? { source: { id: "" }}).source ?? {id: ""}).id ?? "") {
      case "viewNotifications":
        this.entitlementExceptions.viewNotifications = value;
        break;

      case "viewUsers":
        this.entitlementExceptions.viewUsers = value;
        break;

      case "viewVouchers":
        this.entitlementExceptions.viewVouchers = value;
        break;

      case "suspendUsers":
        this.entitlementExceptions.suspendUsers = value;
        break;

      case "refundVouchers":
        this.entitlementExceptions.refundVouchers = value;
        break;

      case "addVoucherTypes":
        this.entitlementExceptions.addVoucherTypes = value;
        break;

      case "addIndustryTypes":
        this.entitlementExceptions.addIndustryTypes = value;
        break;

      case "statisticalReport":
        this.entitlementExceptions.statisticalReport = value;
        break;

      case "auditReport":
        this.entitlementExceptions.auditReport = value;
        break;

      case "financialReport":
        this.entitlementExceptions.financialReport = value;
        break;

      case "authorizeMakerChecker":
        this.entitlementExceptions.authorizeMakerChecker = value;
        break;

      case "entitlementChange":
        this.entitlementExceptions.entitlementChange = value;
        break;

      case "transferVouchers":
        this.entitlementExceptions.transferVouchers = value;
        break;

      case "auditLogs":
        this.entitlementExceptions.auditLogs = value;
        break;

      case "accountingLogs":
        this.entitlementExceptions.accountingLogs = value;
        break;

      case "viewMerchants":
        this.entitlementExceptions.viewMerchants = value;

      case "addMerchants":
        this.entitlementExceptions.addMerchants = value;
        break;

      case "updateMerchants":
        this.entitlementExceptions.updateMerchants = value;
        break;

      case "clear":
        this.entitlementExceptions = new EntitilementExceptionsModel();
        break;

      default: return; // ignore other things that trigger this
    };
}

  booleanSwitch(value: boolean) : boolean { // if false return true if true return false
    return value ? true : false || value ? false : true; // -- I like one liners
  }

}
