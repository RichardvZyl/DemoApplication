import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { AppButtonModule } from "../../../components/button/button.module";
import { AppLabelModule } from "../../../components/label/label.module";
import { AppSelectCommentModule } from "../../..//components/select/comment/comment.module";
import { AppSelectPostModule } from "../../..//components/select/post/post.module";
import { AppSelectUserModule } from "../../..//components/select/user/user.module";
import { AppFormComponent } from "./form.component";
import { AppRouteGuard } from "../../../core/guards/route.guard";

const routes: Routes = [
    { path: "", component: AppFormComponent, canActivate: [AppRouteGuard] }
];

@NgModule({
    declarations: [AppFormComponent],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forChild(routes),
        AppButtonModule,
        AppLabelModule,
        AppSelectCommentModule,
        AppSelectPostModule,
        AppSelectUserModule
    ],
    exports: [RouterModule]
})
export class AppFormModule { }
