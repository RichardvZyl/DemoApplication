import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { AppButtonModule } from "../../components/button/button.module";
import { AppInputPasswordModule } from "../../components/input/password/password.module";
import { AppInputTextModule } from "../../components/input/text/text.module";
import { AppLabelModule } from "../../components/label/label.module";
import { AppSignInComponent } from "./signin.component";

const routes: Routes = [
    { path: "", component: AppSignInComponent }
];

@NgModule({
    declarations: [AppSignInComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild(routes),
        AppButtonModule,
        AppInputPasswordModule,
        AppInputTextModule,
        AppLabelModule
    ],
    exports: [RouterModule]
})
export class AppSignInModule { }
