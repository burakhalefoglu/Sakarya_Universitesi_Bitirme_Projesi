import { Component, OnInit } from '@angular/core';
import { LocalStorageService } from 'app/core/services/local-storage.service';
import { LoginUser } from '../../../model/login-user';
import { AuthService } from './services/Auth.service';
import {AlertifyService} from '../../../services/Alertify.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  username = '' ;
  loginUser: LoginUser = new LoginUser();


  constructor(private auth: AuthService,
    private storageService: LocalStorageService,
              private  alertifyService: AlertifyService) { }

  ngOnInit() {
    this.username = this.auth.userName;
  }

  login() {
    if (this.loginUser.password === undefined
        || this.loginUser.email === undefined) {

      this.alertifyService.error('Please fill all fields');
      return;
    }
    this.auth.login(this.loginUser);
  }

  logOut() {
      this.storageService.removeToken();
  }

}
