import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { RouteService } from 'app/services/route.service';
import { Observable } from 'rxjs';
import { LocalStorageService } from '../services/local-storage.service';


@Injectable()
export class LoginGuard implements CanActivate {

    constructor(private router: Router, private routeService: RouteService,storageService: LocalStorageService) { }

    canActivate(route: ActivatedRouteSnapshot,
                state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        console.log(this.routeService.loggedIn());
        if (this.routeService.loggedIn()) {
            return true;
        }
        this.router.navigate(['auth']);
        return false;

    }


}
