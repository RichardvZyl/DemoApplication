import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppFooterComponent } from "./footer/footer.component";
import { AppHeaderComponent } from "./header/header.component";
import { AppLayoutMainComponent } from "./layout-main/layout-main.component";
import { AppLayoutComponent } from "./layout/layout.component";
import { AppNavComponent } from "./nav/nav.component";

// table imports
import { UserModule } from "./../views/user/user.module";
import { NotificationModule } from "./../views/notification/notification.module";

// popup imports
import { AuthLayoutComponent } from "./auth-layout/auth-layout.component";

import { MatSidenavModule } from "@angular/material/sidenav";

@NgModule({
    declarations: [
        AppFooterComponent,
        AppHeaderComponent,
        AppLayoutComponent,
        AppLayoutMainComponent,
        AppNavComponent,
        AuthLayoutComponent
    ],
    imports: [
        RouterModule,
        UserModule,
        NotificationModule,
        MatSidenavModule,
    ],
    exports: [AppFooterComponent, AppLayoutComponent, AppHeaderComponent, AppLayoutMainComponent, AppNavComponent]
})
export class AppLayoutsModule { }