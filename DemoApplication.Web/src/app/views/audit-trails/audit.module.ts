import { AuditComponent } from "./audit.component";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgbdModalAuditContentComponent } from "./audit-popup.component";
import { AppRouteGuard } from "../../core/guards/route.guard";

const routes: Routes = [
    { path: "", component: AuditComponent, canActivate: [AppRouteGuard]  }
];

@NgModule({
    declarations: [AuditComponent, NgbdModalAuditContentComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        MatTableModule,
        MatSortModule,
        MatPaginatorModule, // for pagination use
        MatFormFieldModule, // for filter input field
        MatInputModule // for filter input
    ],
    exports: [RouterModule, AuditComponent]
})

export class AuditModule { }
