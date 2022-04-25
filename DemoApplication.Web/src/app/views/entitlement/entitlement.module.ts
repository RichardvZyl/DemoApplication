import { EntitlementComponent } from "./entitlement.component";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { RouterModule, Routes } from "@angular/router";
import { AppButtonModule } from "../../components/button/button.module";
import { AppLabelModule } from "../../components/label/label.module";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule } from "@angular/material/dialog";
import { MatInputModule } from "@angular/material/input";
import { AppInputTextModule } from "../../components/input/text/text.module";
import { AppRouteGuard } from "../../core/guards/route.guard";

const routes: Routes = [
    { path: "", component: EntitlementComponent, canActivate: [AppRouteGuard] }
];

@NgModule({
    declarations: [EntitlementComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild(routes),
        AppButtonModule,
        AppInputTextModule,
        AppLabelModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDialogModule,
        MatCheckboxModule,
        FormsModule
    ],
    exports: [RouterModule]
})

export class EntitlementModule { }




