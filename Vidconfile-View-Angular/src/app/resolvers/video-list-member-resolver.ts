import { Injectable } from '@angular/core';
import { Video } from '../models/video';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { RouterService } from '../services/router.service';

@Injectable()
export class VideoListMemberResolver implements Resolve<Video> {
    constructor(private videoService: VideoService,
        private router: Router,
        private alertify: AlertifyService,
        private routerService: RouterService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Video> {
        return this.videoService.getById(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.routerService.navigateHome();
                return of(null);
            })
        );
    }
}
