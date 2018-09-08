import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpEvent, HttpRequest, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from '../models/video';
import { UploadVideoModel } from '../models/upload-video-model';

const httpOptions = {
  headers: new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('token')
  })
};

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getVideos(): Observable<Video[]> {
    return this.http.get<Video[]>(this.baseUrl + 'videos', httpOptions);
  }

  // file from event.target.files[0]
  uploadVideo(uploadVideoModel: UploadVideoModel): Observable<HttpEvent<any>> {

    const formData = new FormData();
    formData.append('upload', uploadVideoModel.video);

    const params = new HttpParams({});

    const options = {
      params: params,
      reportProgress: true,
    };

    const req = new HttpRequest('POST', this.baseUrl + 'videos/uploadvideo', formData, options);
    return this.http.request(req);
  }
}
