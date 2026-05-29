import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserService } from '../ecommerce/services/user.service';
import { SnackbarService } from '../ecommerce/services/snackbar.service';
 

export const adminGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);
  const snack = inject(SnackbarService);

  if (userService.isAdmin()) {
    return true;
  } else {
    snack.error('Nope');
    router.navigateByUrl('/products');
    return false;
  }
};
