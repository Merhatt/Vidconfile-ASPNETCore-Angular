import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { EditProfileComponent } from 'src/app/edit-profile/edit-profile.component';
import { AuthGuard } from './guards/auth.guard';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'edit-profile', component: EditProfileComponent, canActivate: [AuthGuard] },
    { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
