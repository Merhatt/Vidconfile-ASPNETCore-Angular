import { Component, OnInit } from '@angular/core';
import { VideoService } from '../services/video.service';
import { AlertifyService } from '../services/alertify.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { UploadVideoModel } from '../models/upload-video-model';

@Component({
  selector: 'app-upload-video',
  templateUrl: './upload-video.component.html',
  styleUrls: ['./upload-video.component.css']
})
export class UploadVideoComponent implements OnInit {
  model: UploadVideoModel = new UploadVideoModel();

  constructor(private videoService: VideoService,
    private alertify: AlertifyService) {}

  ngOnInit() {
  }

  selectVideo(event) {
    // this.uploadFile(event.target.files);
    this.model.video = event.target.files[0];
  }

  uploadVideo() {
    if (!this.model.video) {
      this.alertify.error('No file selected');
      return;
    }

    this.videoService.uploadVideo(this.model)
      .subscribe(
        event => {
          if (event.type === HttpEventType.UploadProgress) {
            const percentDone = Math.round(100 * event.loaded / event.total);
            this.alertify.warning(`File is ${percentDone}% loaded.`);
          } else if (event instanceof HttpResponse) {
            this.alertify.success('File is completely loaded!');
          }
        },
        (err) => {
          this.alertify.error('Upload Error: ' + err);
        }, () => {
          this.alertify.success('Upload done');
        }
      );
  }
}
