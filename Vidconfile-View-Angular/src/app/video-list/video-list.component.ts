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
  searchInput: string;

  constructor(private videoService: VideoService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadVideos();
    this.searchInput = '';
  }

  loadVideos(): void {
    this.videoService.getVideos()
        .subscribe((videos: Video[]) => {
          this.videos = videos;

          this.videos.sort((a, b) => {
            return a.created > b.created ? -1 : a.created < b.created ? 1 : 0;
          });
        }, error => {
          this.alertify.error(error);
        });
  }

  videoSearchFilter(video: Video, search: string): boolean {
    return video.title.includes(search);
  }
}
