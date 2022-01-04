import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  isAuth = true;
  name = 'Register';
  constructor() { }

  ngOnInit(): void {
  }

  public changeAuthStatus(): void {
    if (this.isAuth) {
      this.isAuth = false;
      this.name = 'Login'
      return;
    }
    this.isAuth = true;
   this.name = 'Register';
  }
}
