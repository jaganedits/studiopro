import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [ButtonModule],
  templateUrl: './unauthorized.html',
  styleUrl: './unauthorized.scss'
})
export class Unauthorized implements OnInit {
  errorCode: number = 401;
  errorTitle: string = 'Unauthorized Access';
  errorMessage: string = 'You need to log in to access this page.';

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Check query params for error type
    this.route.queryParams.subscribe(params => {
      const code = params['code'];
      if (code === '403') {
        this.errorCode = 403;
        this.errorTitle = 'Access Forbidden';
        this.errorMessage = 'You don\'t have permission to access this resource. Contact your administrator if you believe this is an error.';
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }

  goBack(): void {
    window.history.back();
  }
}
