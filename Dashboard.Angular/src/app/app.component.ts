import { Component } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import { AuthService } from './components/auth/login/services/auth.service';

export let browserRefresh = false;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent  {

  subscription: Subscription;
  isRefresh: boolean;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    if (!this.authService.loggedIn()) {
      this.authService.logOut();
      this.router.navigateByUrl('/auth');
    }
    this.router.navigateByUrl('/dashboard')

    this.subscription = router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        browserRefresh = !router.navigated;
      }
  });
  }


  isLoggedIn(): boolean {
    return this.authService.loggedIn();
  }

}
