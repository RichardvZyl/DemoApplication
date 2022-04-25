import { Component, Input, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { MakerCheckerModel } from "./../../models/auth/maker-checker.model";
import { AppAuthService } from "./../../services/auth.service";
import { MakerCheckerActionsEnum } from "../../models/enums.model";
import { AppFileService } from "../../services/file.service";
import { FileModel } from "../../models/file/file.model";
import { HttpResponse } from "@angular/common/http";
// import { saveAs } from "file-saver/FileSaver";
// import { AppFileService } from "./../../services/file.service";

@Component({
  selector: "app-ngbd-modal-update-content",
  templateUrl: "maker-checker-popup.component.html"
})

// TODO this is working but input is very hacky // needs more work
// could not get forms to work
export class NgbdModalMakerCheckerContentComponent implements OnInit {
  @Input() makerChecker: MakerCheckerModel = new MakerCheckerModel();
  fileToUpload: File = null as any;
  makerCheckerActions = MakerCheckerActionsEnum;
  filesForDownload: FileModel[] = [];

  // TODO still require service import to send new values to service // but first figure out why form is not working
  constructor(
    public activeModal: NgbActiveModal,
    private readonly authService: AppAuthService,
    private readonly fileDownloadService: AppFileService
  ) {
    console.log("maker checker popup constructor");
  }

  ngOnInit() {
    ((this.makerChecker ?? { makerChecker: { files: [] }}).files ?? []).forEach((element, index) => {
      this.filesForDownload.push(
        new FileModel(
          element,
          (((this.makerChecker ?? {})).fileNames ?? [])[index] ?? ""));
    });
  }

  isEnabled(): boolean {
    return true;
  }

  async downloadFile(file: FileModel) {
    console.log(file.id);

    await this.fileDownloadService.downloadFile(file.id ?? "").then((data: HttpResponse<Blob>) => {
      console.log(data);

      // const contentDisposition = data.headers.get("content-disposition");
      // const filename = this.getFilenameFromContentDisposition(contentDisposition ?? "");
      const blob = data.body;
      const url = window.URL.createObjectURL(blob);
      const anchor = document.createElement("a");
      anchor.download = file.name ?? "";  // "MakerCheckerFile." + (data.body ?? {}).type ?? ""; // should be filename but unable to retrieve atm
      anchor.href = url;
      anchor.click();
    });
  }

  // getFilenameFromContentDisposition(contentDisposition: string): string {
  //   const regex = /filename=(?<filename>[^,;]+);/g;
  //   const match = regex.exec(contentDisposition);
  //   const filename = ((match ?? { match: {} }).groups ?? {}).filename ?? "";
  //   return filename;
  // }

  acceptChanges() {
    // accept selected maker checker
    console.log("accept changes in Maker Checker");

    this.makerChecker.checkerDate = new Date();
    // this.makerChecker.checkerUser = ""; // get user happens in backend
    this.makerChecker.accepted = true;

    this.authService.actionMakerChecker(this.makerChecker, true);

    this.activeModal.dismiss();
  }

  declineChanges() {
    // decline selected maker checker
    console.log("Decline changes in Maker Checker");

    this.makerChecker.checkerDate = new Date();
    // this.makerChecker.checkerUser = ""; // get user happens in backend
    this.makerChecker.accepted = false;

    this.authService.actionMakerChecker(this.makerChecker, false);

    this.activeModal.dismiss();
  }
}