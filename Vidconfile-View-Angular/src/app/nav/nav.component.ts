import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private routerService: RouterService) { }

  ngOnInit() {
  }

  isLoggedIn() {
    return this.authService.isLogged();
  }

  login() {
    console.log(this.model);
    this.authService.login(this.model)
        .subscribe(next => {
          console.log('Succesfully loged in');
        }, error => {
          console.log(error);
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
