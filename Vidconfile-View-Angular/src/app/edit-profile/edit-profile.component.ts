import { Component, OnInit } from '@angular/core';
import { EditProfileModel } from '../models/edit-profile-model';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import { RouterService } from '../services/router.service';

@Component ({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})

export class EditProfileComponent implements OnInit {
  model: EditProfileModel = new EditProfileModel();

  constructor(private authService: AuthService, private alertify: AlertifyService, private routerService: RouterService) { }

  ngOnInit() {
  }


  public edit() {
    this.authService.editProfile(this.model).subscribe(() => {
      this.alertify.success('Profile updated');
      this.routerService.navigateHome();
    }, error => {
      this.alertify.error(error);
    });
  }
}
