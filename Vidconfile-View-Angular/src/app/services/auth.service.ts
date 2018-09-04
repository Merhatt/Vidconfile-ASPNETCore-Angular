import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  loginUrl = 'http://localhost:5000/api/auth/login';
  registerUrl = 'http://localhost:5000/api/auth/register';

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.loginUrl, model)
    .pipe(
      map((response: any) => {
        const user = response;

        if(user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  logOut() {
    localStorage.removeItem('token');
  }

  isLogged() {
    return !!localStorage.getItem('token');
  }

  register(model: any) {
    return this.http.post(this.registerUrl, model);
  }
}
