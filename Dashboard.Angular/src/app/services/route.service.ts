import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class RouteService {

    jwtHelper: JwtHelperService = new JwtHelperService();

    constructor(private storageService: LocalStorageService,
        private router: Router) { }

    logOut() {
        this.storageService.removeToken();
      }
    
      loggedIn(): boolean {
    
        const isExpired = this.jwtHelper.isTokenExpired(this.storageService.getToken());
        return !isExpired;
      }
}