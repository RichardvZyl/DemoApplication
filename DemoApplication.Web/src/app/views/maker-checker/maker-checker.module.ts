import { MakerCheckerComponent } from "./maker-checker.component";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgbdModalMakerCheckerContentComponent } from "./maker-checker-popup.component";
import { NgbdModalMotivationContentComponent } from "./motivation-popup.component"
import { AppRouteGuard } from "../../core/guards/route.guard";

const routes: Routes = [
    { path: "", component: MakerCheckerComponent, canActivate: [AppRouteGuard] }
];

@NgModule({
    declarations: [MakerCheckerComponent, NgbdModalMakerCheckerContentComponent, NgbdModalMotivationContentComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        MatTableModule, // for mat-table use
        MatSortModule, // for mat-sort use
        MatPaginatorModule, // for pagination use
        MatFormFieldModule, // for filter input field
        MatInputModule // for filter input
    ],
    exports: [RouterModule]
})

export class MakerCheckerModule { }
