import { Component, OnInit } from '@angular/core';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-upload-video',
  templateUrl: './upload-video.component.html',
  styleUrls: ['./upload-video.component.css']
})
export class UploadVideoComponent implements OnInit {
  model: any;

  constructor(private videoService: VideoService,
    private alertify: AlertifyService) {}

  ngOnInit() {
  }

  uploadVideo(event) {
    console.log('upload');
    console.log('event');
  }
}
