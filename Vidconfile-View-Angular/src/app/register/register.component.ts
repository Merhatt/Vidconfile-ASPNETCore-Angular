import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import { RegisterModel } from '../models/register-model';
import { RouterService } from '../services/router.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  model: RegisterModel = new RegisterModel();

  constructor(private authService: AuthService, private alertify: AlertifyService, private routerService: RouterService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('Registered');
      this.routerService.navigateHome();
    }, error => {
      this.alertify.error(error);
    });
  }
}
