import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private routerService: RouterService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  isLoggedIn() {
    return this.authService.isLogged();
  }

  login() {
    this.authService.login(this.model)
        .subscribe(next => {
          this.alertify.success('Succesfully logged in');
          this.routerService.navigateHome();
        }, error => {
          this.alertify.error(error);
        });
  }

  logout() {
    this.authService.logOut();
    this.routerService.navigateHome();
  }

  getUsername() {
    return this.authService.getUsername();
  }

}
