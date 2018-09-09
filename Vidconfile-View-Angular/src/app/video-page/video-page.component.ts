import { Component, OnInit } from '@angular/core';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';
import { RouterService } from '../services/router.service';
import { ActivatedRoute } from '@angular/router';
import { Video } from '../models/video';

@Component({
  selector: 'app-video-page',
  templateUrl: './video-page.component.html',
  styleUrls: ['./video-page.component.css']
})
export class VideoPageComponent implements OnInit {
  video: Video;

  constructor(private videoService: VideoService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadVideo();
  }

  private loadVideo(): void {
    const id = this.route.snapshot.params['id'];

    this.videoService.getById(id).subscribe((video: Video) => {
      this.video = video;
    }, error => {
      this.alertify.error(error);
    });

    console.log(id);
  }
}
