import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

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

}
