import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
  export class RouterService {

  constructor(private router: Router, private route: ActivatedRoute) { }

  navigateHome(): void {
    this.router.navigate(['/']);
  }

  navigateEditProfile(): void {
    this.router.navigate(['/edit-profile']);
  }
}
