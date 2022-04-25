import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AppListComponent } from "./list.component";

const routes: Routes = [
    { path: "", component: AppListComponent }
];

@NgModule({
    declarations: [AppListComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AppListModule { }
