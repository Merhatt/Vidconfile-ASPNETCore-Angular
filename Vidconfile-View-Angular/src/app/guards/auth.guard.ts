import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service';
import { AlertifyService } from '../services/alertify.service';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService,
              private routerService: RouterService,
              private alertify: AlertifyService) {}

  canActivate():  boolean {
    if (this.authService.isLogged()) {
      return true;
    }

    this.alertify.error('You dont have permission to be on this page');

    this.routerService.navigateHome();

    return false;
  }
}
