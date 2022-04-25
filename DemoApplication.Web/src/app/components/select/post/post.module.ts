import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AppSelectPostComponent } from "./post.component";

@NgModule({
    declarations: [
        AppSelectPostComponent
    ],
    exports: [
        AppSelectPostComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class AppSelectPostModule { }
