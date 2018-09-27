import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RegisterModel } from '../models/register-model';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IsSubscribedModel } from '../models/is-subscribed-model';
import { EditProfileModel } from '../models/edit-profile-model';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  baseUrl = environment.apiUrl;

  JwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  public static getToken() {
    return localStorage.getItem('token');
  }

  public login(model: any) {
    return this.http.post(this.baseUrl + 'auth/login', model)
    .pipe(
      map((response: any) => {
        const user = response;

        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  public logOut() {
    localStorage.removeItem('token');
  }

  public isLogged(): boolean {
    const token = localStorage.getItem('token');

    return !this.JwtHelper.isTokenExpired(token);
  }

  public isLoggedCloud(): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'auth/verifytoken');
  }

  public register(model: RegisterModel) {
    return this.http.post(this.baseUrl + 'auth/register', model);
  }

  public getUsername(): string {
    const token = localStorage.getItem('token');

    if (token) {
      const decodedToken = this.JwtHelper.decodeToken(token);
      return decodedToken.unique_name;
    }

    return null;
  }

  public getUserByVideoId(videoId: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/getbyvideoid?id=' + videoId);
  }

  public subscribe(userToSubscribeTo: string): Observable<any> {
    return this.http.get(this.baseUrl + 'users/subscribe?userToSubscribeTo=' + userToSubscribeTo);
  }

  public unsubscribe(userToUnsubscribeTo: string): Observable<any> {

    return this.http.get(this.baseUrl + 'users/unsubscribe?userToUnsubscribeTo=' + userToUnsubscribeTo);
  }

  public isSubscribed(isSubscribedUser: string): Observable<IsSubscribedModel> {
    return this.http.get<IsSubscribedModel>(this.baseUrl + 'users/issubscribed?isSubscribedUser=' + isSubscribedUser);
  }

  public editProfile(editProfileModel: EditProfileModel): Observable<any> {
    return this.http.post(this.baseUrl + 'users/edit', editProfileModel);
  }

  public getById(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/getuser?id=' + id);
  }

  public getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users');
  }
}
