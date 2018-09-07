import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from '../models/video';

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
}
