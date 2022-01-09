import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RouteService } from 'app/services/route.service';
import { AuthService } from '../auth/login/services/Auth.service';


@Component({
selector: 'app-navbar',
templateUrl: './navbar.component.html',
styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

userName: string;
constructor(private routeService: RouteService, 
    private authService: AuthService, private router: Router) {}

isLoggedIn(): boolean {

return this.routeService.loggedIn();
}

logOut() {
this.routeService.logOut();
this.router.navigateByUrl('/auth');

}

help(): void {
window.open(
'https://www.github.com/burakhalefoglu',
'_blank'
);
}
ngOnInit() {
console.log(this.userName);
this.userName = this.authService.getUserName();
}

setUserName() {

this.userName = this.authService.getUserName();
}
}
