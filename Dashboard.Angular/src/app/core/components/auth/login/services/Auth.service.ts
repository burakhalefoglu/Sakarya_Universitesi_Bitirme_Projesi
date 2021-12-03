import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AlertifyService } from 'app/core/services/Alertify.service';
import { LocalStorageService } from 'app/core/services/local-storage.service';
import { environment } from 'environments/environment';
import { LoginUser } from '../../../../model/login-user';
import { TokenModel } from '../../../../model/token-model';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  userName: string;
  decodedToken: any;
  userToken: string;
  jwtHelper: JwtHelperService = new JwtHelperService();

  constructor(private httpClient: HttpClient,
              private storageService: LocalStorageService,
              private router: Router,
              private alertifyService: AlertifyService) {}

  login(loginUser: LoginUser) {

    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json')

    this.httpClient.post<TokenModel>(environment.getApiUrl + '/Auth/login',
        loginUser, { headers: headers }).subscribe(data => {
      if (data.success) {

        this.storageService.setToken(data.data.token);
         const decode = this.jwtHelper.decodeToken(this.storageService.getToken());

        const propUserName = Object.keys(decode).filter(x => x.endsWith('/name'))[0];
        this.userName = decode[propUserName];

        this.router.navigateByUrl('/dashboard');
      } else {
        this.alertifyService.warning(data.message);
      }
    });

  }

  getUserName(): string {
    return this.userName;
  }

  logOut() {
    this.storageService.removeToken();
  }

  loggedIn(): boolean {

    const isExpired = this.jwtHelper.isTokenExpired(this.storageService.getToken());
    return !isExpired;
  }
}
