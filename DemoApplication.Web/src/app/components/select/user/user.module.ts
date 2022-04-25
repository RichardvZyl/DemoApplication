import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AppSelectUserComponent } from "./user.component";

@NgModule({
    declarations: [
        AppSelectUserComponent
    ],
    exports: [
        AppSelectUserComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class AppSelectUserModule { }
