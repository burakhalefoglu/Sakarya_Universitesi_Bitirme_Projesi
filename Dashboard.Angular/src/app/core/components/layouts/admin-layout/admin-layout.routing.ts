import { Routes } from '@angular/router';
import { LoginGuard } from 'app/core/guards/login-guard';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import {AuthComponent} from '../../auth/auth.component';

export const AdminLayoutRoutes: Routes = [

    { path: 'dashboard',      component: DashboardComponent, canActivate: [LoginGuard] },
    { path: 'auth',          component: AuthComponent },

];
