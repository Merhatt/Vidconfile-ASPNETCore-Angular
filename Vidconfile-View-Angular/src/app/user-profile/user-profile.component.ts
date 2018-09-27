import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { AlertifyService } from '../services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { VideoService } from '../services/video.service';
import { Video } from '../models/video';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  public user: User;
  public isSubscribed: boolean;

  constructor(private alertify: AlertifyService, private route: ActivatedRoute,
    private authService: AuthService, private videoService: VideoService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];

      this.user.videos.sort((a, b) => {
        return a.created > b.created ? -1 : a.created < b.created ? 1 : 0;
      });
    });

    this.setIsSubscribed();
  }

  public isLogged(): boolean {
    return this.authService.isLogged();
  }

  private setIsSubscribed(): void {
    if (!this.authService.isLogged()) {
      return;
    }

    this.authService.isSubscribed(this.user.id)
      .subscribe(res => {
        this.isSubscribed = res.isSubscribed;
      }, error => {
        this.alertify.error(error);
      });
  }

  public subscribe(): void {
    this.authService.subscribe(this.user.id)
      .subscribe((res) => {
        this.isSubscribed = true;
        this.user.subscriberCount++;
      }, (error) => {
        this.alertify.error(error);
      });
  }

  public unsubscribe(): void {
    this.authService.unsubscribe(this.user.id)
      .subscribe((res) => {
        this.isSubscribed = false;
        this.user.subscriberCount--;
      }, (error) => {
        this.alertify.error(error);
      });
  }
}
