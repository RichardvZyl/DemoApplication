import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AppHomeComponent } from "./home.component";
import { AppLayoutsModule } from "./../../../layouts/layouts.module";
import { AppRouteGuard } from "../../../core/guards/route.guard";

const routes: Routes = [
    { path: "", component: AppHomeComponent, canActivate: [AppRouteGuard] }
];

@NgModule({
    declarations: [AppHomeComponent],
    imports: [
        RouterModule.forChild(routes),
        AppLayoutsModule,
    ],
    exports: [RouterModule]
})

export class AppHomeModule { }
