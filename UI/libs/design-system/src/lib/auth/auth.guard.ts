import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard {
  authService = inject(AuthService);
  router = inject(Router);

  canActivate(): boolean {
    return this.authService.tieneToken()
      ? true
      : (this.router.navigate(['/login']), false);
  }
}
