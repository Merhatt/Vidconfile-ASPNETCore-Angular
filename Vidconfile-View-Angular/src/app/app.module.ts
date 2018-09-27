import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { AngularFileUploaderModule } from 'angular-file-uploader';
import { JwtModule } from '@auth0/angular-jwt';
import { VgCoreModule } from 'videogular2/core';
import { VgControlsModule } from 'videogular2/controls';
import { VgOverlayPlayModule } from 'videogular2/overlay-play';
import { VgBufferingModule } from 'videogular2/buffering';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './services/auth.service';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { ErrorInterceptorProvider } from './services/error.interceptor';
import { AlertifyService } from './services/alertify.service';
import { appRoutes } from './routes';
import { EditProfileComponent } from 'src/app/edit-profile/edit-profile.component';
import { RouterService } from './services/router.service';
import { AuthGuard } from './guards/auth.guard';
import { VideoService } from './services/video.service';
import { VideoListComponent } from './video-list/video-list.component';
import { UploadVideoComponent } from './upload-video/upload-video.component';
import { VideoListMemberComponent } from './video-list-member/video-list-member.component';
import { environment } from '../environments/environment';
import { VideoPageComponent } from './video-page/video-page.component';
import { VideoListMemberResolver } from './resolvers/video-list-member-resolver';
import { VideoListMemberUserResolver } from './resolvers/video-list-member-user-resolver';
import { CommentComponent } from './comment/comment.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileResolver } from './resolvers/user-profile-resolver';
import { UserListComponent } from './user-list/user-list.component';
import { UserListMemberComponent } from './user-list-member/user-list-member.component';



@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      RegisterComponent,
      HomeComponent,
      EditProfileComponent,
      VideoListComponent,
      UploadVideoComponent,
      VideoListMemberComponent,
      VideoPageComponent,
      CommentComponent,
      UserProfileComponent,
      UserListComponent,
      UserListMemberComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      AngularFileUploaderModule,
      JwtModule.forRoot({
            config: {
                  tokenGetter: AuthService.getToken,
                  whitelistedDomains: environment.whitelistedAuthUrls
            }
      }),
      VgCoreModule,
      VgControlsModule,
      VgOverlayPlayModule,
      VgBufferingModule
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      RouterService,
      AuthGuard,
      VideoService,
      VideoListMemberResolver,
      VideoListMemberUserResolver,
      UserProfileResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
