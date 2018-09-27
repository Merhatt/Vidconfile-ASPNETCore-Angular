import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { User } from '../models/user';
import { RouterService } from '../services/router.service';

@Injectable()
export class VideoListMemberUserResolver implements Resolve<User> {
    constructor(private authService: AuthService,
        private router: Router,
        private alertify: AlertifyService,
        private routerService: RouterService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.authService.getUserByVideoId(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.routerService.navigateHome();
                return of(null);
            })
        );
    }
}
