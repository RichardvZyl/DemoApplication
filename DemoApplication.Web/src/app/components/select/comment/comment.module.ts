import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AppSelectCommentComponent } from "./comment.component";

@NgModule({
    declarations: [
        AppSelectCommentComponent
    ],
    exports: [
        AppSelectCommentComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class AppSelectCommentModule { }
