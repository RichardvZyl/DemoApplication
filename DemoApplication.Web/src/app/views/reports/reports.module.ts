import { ReportsComponent } from "./reports.component";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { RouterModule, Routes } from "@angular/router";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { AppButtonModule } from "../../components/button/button.module";
import { AppInputTextModule } from "../../components/input/text/text.module";
import { AppLabelModule } from "../../components/label/label.module";
import { MatDialogModule } from "@angular/material/dialog";
import { AppRouteGuard } from "../../core/guards/route.guard";


const routes: Routes = [
    { path: "", component: ReportsComponent, canActivate: [AppRouteGuard]  }
];

@NgModule({
    declarations: [ReportsComponent],
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
        MatDatepickerModule,
        MatNativeDateModule,
        MatDialogModule
    ],
    exports: [RouterModule]
})

export class ReportsModule { }