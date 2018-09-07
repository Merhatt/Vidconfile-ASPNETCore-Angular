import { Component, OnInit } from '@angular/core';
import { Video } from '../models/video';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css']
})
export class VideoListComponent implements OnInit {
  videos: Video[];

  constructor(private videoService: VideoService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadVideos();
  }

  loadVideos() {
    this.videoService.getVideos()
        .subscribe((videos: Video[]) => {
          this.videos = videos;
        }, error => {
          this.alertify.error(error);
        });
  }
}
