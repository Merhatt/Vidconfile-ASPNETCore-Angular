import { Injectable } from '@angular/core';
import { Video } from '../models/video';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';
import { RouterService } from '../services/router.service';

@Injectable()
export class UserProfileResolver implements Resolve<User> {
    constructor(private authServices: AuthService,
        private router: Router,
        private alertify: AlertifyService,
        private routerService: RouterService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.authServices.getById(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.routerService.navigateHome();
                return of(null);
            })
        );
    }
}
