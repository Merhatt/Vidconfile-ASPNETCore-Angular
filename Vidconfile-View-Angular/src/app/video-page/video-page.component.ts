import { Component, OnInit } from '@angular/core';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';
import { RouterService } from '../services/router.service';
import { ActivatedRoute } from '@angular/router';
import { Video } from '../models/video';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';
import { VideoComment } from '../models/video-comment';
import { SendCommentModel } from '../models/send-comment-model';

@Component({
  selector: 'app-video-page',
  templateUrl: './video-page.component.html',
  styleUrls: ['./video-page.component.css']
})
export class VideoPageComponent implements OnInit {
  public video: Video;
  public user: User;
  public isSubscribed: boolean;
  public videoComments: VideoComment[];
  public showComments: boolean;
  public sendCommentModel = new SendCommentModel();

  constructor(private alertify: AlertifyService, private route: ActivatedRoute,
    private authService: AuthService, private videoService: VideoService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.video = data['video'];
      this.user = data['user'];
    });

    this.setIsSubscribed();

    this.showComments = false;
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

  public isLogged(): boolean {
    return this.authService.isLogged();
  }

  public loadComments(): void {
    this.videoService.getAllComments(this.video.id)
      .subscribe((res: VideoComment[]) => {
        this.videoComments = res;

        this.showComments = true;

        this.sortComments(this.videoComments);
      }, (error) => {
        this.alertify.error(error);
      });
  }

  public uploadComment(): void {
    this.sendCommentModel.videoId = this.video.id;

    this.videoService.uploadComment(this.sendCommentModel)
      .subscribe((res) => {
        this.alertify.success('Comment added');

        this.videoComments.push(res);

        this.sendCommentModel.commentText = '';

        this.sortComments(this.videoComments);
      }, (error) => {
        this.alertify.error(error);
      });
  }

  private sortComments(comments: VideoComment[]): void {
    comments.sort((a, b) => {
      return a.created > b.created ? -1 : a.created < b.created ? 1 : 0;
    });
  }
}
