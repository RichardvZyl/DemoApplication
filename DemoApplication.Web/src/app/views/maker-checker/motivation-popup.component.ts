import { Component, Output, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { AppFileService } from "../../services/file.service";
import { NewMakerCheckerModel } from "../../models/auth/new-maker-checker.model";
import { FileUploadModel } from "../../models/file/file.upload.model";
// import { FormGroup, FormControl } from "@angular/forms";
// import { FileModel } from "../../models/file/file.model";

@Component({
  selector: "app-ngbd-modal-update-content",
  templateUrl: `./motivation-popup.component.html`
})

// TODO this is working but input is very hacky // needs more work
// could not get forms to work
export class NgbdModalMotivationContentComponent {
    @Input() input: any;
    @Output() motivation: NewMakerCheckerModel = new NewMakerCheckerModel();
    filesForUpload: FileUploadModel[] = [];
    fileIds: string[] = [];

    constructor(
        public activeModal: NgbActiveModal,
        private readonly fileUploadService: AppFileService
        ) {
          console.log("motivation popup constructor");
    }

    isEnabled() : boolean { // TODO disable button if no file uploaded?
      return true; // (this.filesToUpload === null) ? false : true;
    }

    handleFileInput(event: any) {
      console.log("File upload method called");

      const files = event.target.files ?? null as any;

      if (!files) {
        console.log("Error occured empty file uploaded");
        return;
      }

      this.fileUploadService.uploadFile(event.target.files[0] as File).then(result => {
        if (result) {
          this.fileIds.push(result);

          // set a visual indication of uploaded files
          this.filesForUpload.push({
            id: result,
            name: files?.item(0).name
          })
        }
      });
    }

    removeFile(file: FileUploadModel) {
      console.log(file.id);

      this.fileUploadService.removeFile(file.id ?? "");

      const index: number = this.filesForUpload.indexOf(file);
      if (index !== -1) {
        this.filesForUpload.splice(index, 1);
      }
    }

    submit(inputMotivation: string){
      // const motivationString = (inputMotivation.target as HTMLInputElement).value ?? "";
      console.log(inputMotivation);

      this.motivation.files = this.fileIds ?? []; // ensure property is not null if no files uploaded
      this.motivation.action = 0; // value is ignored as defined based on url called
      this.motivation.motivation = inputMotivation;
      this.motivation.model = JSON.stringify(this.input); // the action performed before motivation popup sent a model to motivation popup

      console.log(this.motivation);

      this.activeModal.close(this.motivation);
    }
}