import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  loginUrl = 'http://localhost:5000/api/auth/login';
  registerUrl = 'http://localhost:5000/api/auth/register';

  JwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.loginUrl, model)
    .pipe(
      map((response: any) => {
        const user = response;

        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  logOut() {
    localStorage.removeItem('token');
  }

  isLogged() {
    const token = localStorage.getItem('token');

    return !this.JwtHelper.isTokenExpired(token);
  }

  register(model: any) {
    return this.http.post(this.registerUrl, model);
  }

  getUsername() {
    const token = localStorage.getItem('token');

    if (token) {
      const decodedToken = this.JwtHelper.decodeToken(token);
      return decodedToken.unique_name;
    }

    return null;
  }
}
