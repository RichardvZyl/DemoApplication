import { Component } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { FileModel } from "./../../../app/models/file/file.model";
import { FileUploadModel } from "./../../../app/models/file/file.upload.model";
import { AppFileService } from "./../../../app/services/file.service";
import { AppBaseComponent } from "../base/base.component";

@Component({
    providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: AppFileComponent, multi: true }],
    selector: "app-file",
    templateUrl: "./file.component.html"
})
export class AppFileComponent extends AppBaseComponent<FileModel[]> {
    constructor(private readonly appFileService: AppFileService) {
        super();
    }

    uploads = new Array<FileUploadModel>();

    change(files: FileList | null) {
        if (!files) { return; }

        for (let index = 0; index < files.length; index++) {
            const file = files.item(index) as File;
            const upload = new FileUploadModel(file.name, 0);
            this.uploads.push(upload);

            // this.appFileService.uploadFile(file).subscribe((result: FileUploadModel) => {
            //     upload.progress = result.progress;
            //     if (result.id) {
            //         this.value.push(new FileModel(result.id, file.name));
            //         this.uploads = this.uploads.filter((x) => (x.progress ?? 1) < 100);
            //     }
            // });
        }
    }
}
