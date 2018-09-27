import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { EditProfileComponent } from 'src/app/edit-profile/edit-profile.component';
import { AuthGuard } from './guards/auth.guard';
import { UploadVideoComponent } from './upload-video/upload-video.component';
import { VideoListComponent } from 'src/app/video-list/video-list.component';
import { VideoPageComponent } from './video-page/video-page.component';
import { VideoListMemberResolver } from './resolvers/video-list-member-resolver';
import { RegisterComponent } from './register/register.component';
import { VideoListMemberUserResolver } from './resolvers/video-list-member-user-resolver';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileResolver } from './resolvers/user-profile-resolver';
import { UserListComponent } from './user-list/user-list.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'videos', component: VideoListComponent },
    { path: 'videos/:id', component: VideoPageComponent, resolve: { video: VideoListMemberResolver, user: VideoListMemberUserResolver } },
    { path: 'users', component: UserListComponent },
    { path: 'users/:id', component: UserProfileComponent, resolve: { user: UserProfileResolver } },
    { path: 'upload-video', component: UploadVideoComponent, canActivate: [AuthGuard] },
    { path: 'edit-profile', component: EditProfileComponent, canActivate: [AuthGuard] },
    { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
