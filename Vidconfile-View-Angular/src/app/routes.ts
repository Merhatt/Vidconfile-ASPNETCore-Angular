import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { EditProfileComponent } from 'src/app/edit-profile/edit-profile.component';
import { AuthGuard } from './guards/auth.guard';
import { UploadVideoComponent } from './upload-video/upload-video.component';
import { VideoListComponent } from 'src/app/video-list/video-list.component';
import { VideoPageComponent } from './video-page/video-page.component';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'videos', component: VideoListComponent },
    { path: 'videos/:id', component: VideoPageComponent },
    { path: 'upload-video', component: UploadVideoComponent, canActivate: [AuthGuard] },
    { path: 'edit-profile', component: EditProfileComponent, canActivate: [AuthGuard] },
    { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
