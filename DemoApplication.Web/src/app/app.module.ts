import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { ErrorHandler, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { AppLayoutsModule } from "./layouts/layouts.module";
import { AppComponent } from "./app.component";
import { AppRoutingModule } from "./app.routing.module";
import { AppErrorHandler } from "./core/handlers/error.handler";
import { AppHttpInterceptor } from "./core/interceptors/http.interceptor";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ReactiveFormsModule } from "@angular/forms";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatDialogModule } from "@angular/material/dialog";
import { MatNativeDateModule } from "@angular/material/core";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatSortModule } from "@angular/material/sort";
import { HashLocationStrategy, LocationStrategy } from "@angular/common";


@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppLayoutsModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatFormFieldModule,
        ReactiveFormsModule, // importing this module in reports module does not work had to import it here to get it working
        MatDatepickerModule,
        MatNativeDateModule,
        MatDialogModule,
        MatTooltipModule,
        MatSortModule
    ],
    providers: [
        { provide: ErrorHandler, useClass: AppErrorHandler },
        { provide: HTTP_INTERCEPTORS, useClass: AppHttpInterceptor, multi: true },
        { provide: LocationStrategy, useClass: HashLocationStrategy } // fix for 404 errors
    ]
})

export class AppModule { }
