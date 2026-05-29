import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { UserService } from '../ecommerce/services/user.service';

export const authGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  // If current user already loaded, allow.
  if (userService.currentUser()) {
    return true;
  }

  // Try to load current user (useful when token exists but app hasn't fetched profile yet)
  return firstValueFrom(userService.getCurrentUser()).then(
    () => true,
    () => {
      router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
  );
};
