import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[];
  searchInput: string;

  constructor(private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadVideos();
    this.searchInput = '';
  }

  loadVideos(): void {
    this.authService.getAllUsers()
        .subscribe((users: User[]) => {
          this.users = users;

          this.users.sort((a, b) => {
            return a.subscriberCount > b.subscriberCount ? -1 : a.subscriberCount < b.subscriberCount ? 1 : 0;
          });
        }, error => {
          this.alertify.error(error);
        });
  }

  userSearchFilter(user: User, search: string): boolean {
    return user.username.includes(search);
  }
}
