import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NotificationComponent } from "./views/notification/notification.component";
import { UserComponent } from "./views/user/user.component";
import { ReportsComponent } from "./views/reports/reports.component";
import { EntitlementComponent } from "./views/entitlement/entitlement.component";
import { MakerCheckerComponent } from "./views/maker-checker/maker-checker.component";
import { AppSignInComponent } from "./views/signin/signin.component";
import { ForgotPasswordComponent } from "./views/forgot-password/forgot-password.component";
import { NewPasswordComponent } from "./views/new-password/new-password.component";
import { RegisterComponent } from "./views/register/register.component";
import { AppLayoutMainComponent } from "./layouts/layout-main/layout-main.component";
import { AuthLayoutComponent } from "./layouts/auth-layout/auth-layout.component";
import { AuditComponent } from "./views/audit-trails/audit.component";
import { AppRouteGuard } from "./core/guards/route.guard";

// import { AppRouteGuard } from "./core/guards/route.guard";
// import { AppLayoutMainComponent } from "./layouts/layout-main/layout-main.component";

const routes: Routes = [
    {
        path: "home", component: AppLayoutMainComponent, canActivate: [AppRouteGuard],
        children: [
            {
                path: "audit", component: AuditComponent,
                loadChildren: () => import("./views/audit-trails/audit.module").then((x) => x.AuditModule)
            },
            {
                path: "notification", component: NotificationComponent,
                loadChildren: () => import("./views/notification/notification.module").then((x) => x.NotificationModule)
            },
            {
                path: "alerts", component: NotificationComponent,
                loadChildren: () => import("./views/notification/notification.module").then((x) => x.NotificationModule)
            },
            {
                path: "user", component: UserComponent,
                loadChildren: () => import("./views/user/user.module").then((x) => x.UserModule)
            },
            {
                path: "reports", component: ReportsComponent,
                loadChildren: () => import("./views/reports/reports.module").then((x) => x.ReportsModule)
            },
            {
                path: "maker-checker", component: MakerCheckerComponent,
                loadChildren: () => import("./views/maker-checker/maker-checker.module").then((x) => x.MakerCheckerModule)
            },
            {
                path: "entitlement", component: EntitlementComponent,
                loadChildren: () => import("./views/entitlement/entitlement.module").then((x) => x.EntitlementModule)
            },
        ]
    },
    {
        path: "", component: AuthLayoutComponent,
        children: [
            {
                path: "signin", component: AppSignInComponent,
                loadChildren: () => import("./views/signin/signin.module").then((x) => x.AppSignInModule)
            },
            {
                path: "forgotPassword", component: ForgotPasswordComponent,
                loadChildren: () => import("./views/forgot-password/forgot-password.module").then((x) => x.ForgotPasswordModule)
            },
            {
                path: "newPassword", component: NewPasswordComponent,
            },
            {
                path: "register", component: RegisterComponent,
                loadChildren: () => import("./views/register/register.module").then((x) => x.RegisterModule)
            }
        ]
    },
    { path: "**", redirectTo: "signin" },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
