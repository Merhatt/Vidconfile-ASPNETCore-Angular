import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpEvent, HttpRequest, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from '../models/video';
import { UploadVideoModel } from '../models/upload-video-model';
import { VideoComment } from '../models/video-comment';
import { SendCommentModel } from '../models/send-comment-model';

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getVideos(): Observable<Video[]> {
    return this.http.get<Video[]>(this.baseUrl + 'videos');
  }

  public getById(id: string): Observable<Video> {
    return this.http.get<Video>(this.baseUrl + 'videos/getbyid?id=' + id);
  }

  public uploadVideo(uploadVideoModel: UploadVideoModel): Observable<HttpEvent<any>> {

    const formData = new FormData();
    formData.append('upload', uploadVideoModel.video);
    formData.append('description', uploadVideoModel.description);
    formData.append('thumbnailUrl', uploadVideoModel.thumbnailUrl);
    formData.append('title', uploadVideoModel.title);

    const params = new HttpParams({});

    const options = {
      params: params,
      reportProgress: true
    };

    const req = new HttpRequest('POST', this.baseUrl + 'videos/uploadvideo', formData, options);
    return this.http.request(req);
  }

  public getAllComments(videoId: string): Observable<VideoComment[]> {
      return this.http.get<VideoComment[]>(this.baseUrl + 'videos/getallcomments?videoId=' + videoId);
  }

  public uploadComment(model: SendCommentModel): Observable<VideoComment> {
      return this.http.post<VideoComment>(this.baseUrl + 'videos/addcomment', model);
  }
}
