import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
// import { MatFormFieldModule, MatInputModule } from "@angular/material";
import { AppButtonModule } from "../../components/button/button.module";
import { AppInputPasswordModule } from "../../components/input/password/password.module";
import { AppInputTextModule } from "../../components/input/text/text.module";
import { AppLabelModule } from "../../components/label/label.module";
import { RegisterComponent } from "./register.component";
import { MatInputModule } from "@angular/material/input";

const routes: Routes = [
    { path: "", component: RegisterComponent }
];

@NgModule({
    declarations: [RegisterComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild(routes),
        AppButtonModule,
        AppInputPasswordModule,
        AppInputTextModule,
        AppLabelModule,
        MatInputModule
    ],
    exports: [RouterModule]
})

export class RegisterModule { }