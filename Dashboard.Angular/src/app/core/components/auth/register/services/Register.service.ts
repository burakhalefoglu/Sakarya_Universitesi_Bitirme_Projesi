import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AlertifyService } from 'app/core/services/Alertify.service';
import { LocalStorageService } from 'app/core/services/local-storage.service';
import { environment } from 'environments/environment';
import {TokenModel} from '../../../../model/token-model';
import {RegisterUser} from '../../../../model/register-user';


@Injectable({
  providedIn: 'root'
})

export class RegisterService {

  userName: string;
  decodedToken: any;
  userToken: string;
  jwtHelper: JwtHelperService = new JwtHelperService();

  constructor(private httpClient: HttpClient,
              private storageService: LocalStorageService,
              private router: Router,
              private alertifyService: AlertifyService) {}

  public register(registerUser: RegisterUser) {

    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json')

    this.httpClient.post<TokenModel>(environment.getApiUrl + '/Auth/register',
        registerUser, { headers: headers }).subscribe(data => {
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
}
