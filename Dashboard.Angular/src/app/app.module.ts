import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { AdminLayoutComponent } from './core/components/layouts/admin-layout/admin-layout.component';
import { AlertifyService } from './core/services/alertify.service';
import { AuthService } from './core/components/auth/login/services/auth.service';
import { LocalStorageService } from './core/services/local-storage.service';
import { LoginGuard } from './core/guards/login-guard';
import { AuthInterceptorService } from './core/interceptors/auth-interceptor.service';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import {RouterModule} from '@angular/router';
import {CommonModule} from '@angular/common';
import {AdminLayoutRoutes} from './core/components/layouts/admin-layout/admin-layout.routing';
import {MatButtonModule} from '@angular/material/button';
import {MatRippleModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatTableModule} from '@angular/material/table';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {DashboardComponent} from './core/components/dashboard/dashboard.component';
import {LoginComponent} from './core/components/auth/login/login.component';
import {FooterComponent} from './core/components/footer/footer.component';
import {NavbarComponent} from './core/components/navbar/navbar.component';
import {RegisterComponent} from './core/components/auth/register/register.component';
import {AuthComponent} from './core/components/auth/auth.component';


export function tokenGetter() {
  return localStorage.getItem('token');
}


@NgModule({
  imports: [
    RouterModule,
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTooltipModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCheckboxModule,
    NgbModule,
    NgMultiSelectDropDownModule,
    SweetAlert2Module,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgMultiSelectDropDownModule.forRoot(),
    SweetAlert2Module.forRoot(),
    NgbModule,
    RouterModule.forRoot([]),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    })],
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    DashboardComponent,
    LoginComponent,
    FooterComponent,
    NavbarComponent,
    RegisterComponent,
    AuthComponent
  ],
  exports: [
    FooterComponent,
    NavbarComponent,
  ],
  providers: [AlertifyService, AuthService, LocalStorageService, LoginGuard, AuthInterceptorService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },
    HttpClient

  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
