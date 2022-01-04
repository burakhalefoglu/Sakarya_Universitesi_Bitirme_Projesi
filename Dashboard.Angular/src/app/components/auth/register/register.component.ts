import { Component, OnInit } from '@angular/core';
import {RegisterService} from './services/Register.service';
import {LocalStorageService} from '../../../services/local-storage.service';
import {RegisterUser} from '../../../models/register-user';
import {AlertifyService} from '../../../services/Alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  username = '' ;
  registerUser: RegisterUser = new RegisterUser();


  constructor(private auth: RegisterService,
              private storageService: LocalStorageService,
              private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.username = this.auth.userName;
  }

  register() {
    if (this.registerUser.password === undefined
        || this.registerUser.passwordAgain === undefined
        || this.registerUser.email === undefined) {

      this.alertifyService.error('Please fill all fields');
      return;
    }

    if (this.registerUser.password !== this.registerUser.passwordAgain) {
      this.alertifyService.error('Passwords do not match');
      return;
    }
    this.auth.register(this.registerUser);
  }

  logOut() {
    this.storageService.removeToken();
  }

}

