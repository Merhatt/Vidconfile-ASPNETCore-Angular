import { Component, OnInit, Input } from '@angular/core';
import { User } from '../models/user';

@Component({
  selector: 'app-user-list-member',
  templateUrl: './user-list-member.component.html',
  styleUrls: ['./user-list-member.component.css']
})
export class UserListMemberComponent implements OnInit {
  @Input() user: User;

  constructor() { }

  ngOnInit() {
  }

}
