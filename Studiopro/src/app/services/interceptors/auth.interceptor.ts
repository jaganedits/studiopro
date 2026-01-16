import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { SKIP_AUTH } from './interceptor.tokens';
import { AuthService } from '../helpers/auth.service';


/**
 * Authentication Interceptor
 * - Injects Bearer token and custom headers
 * - Handles 401 unauthorized responses
 * - Supports skipping auth via context token
 */
export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  const authService = inject(AuthService);

  // Skip auth header injection if explicitly requested
  if (req.context.get(SKIP_AUTH)) {
    return next(req);
  }

  // Clone request and add auth headers
  const authReq = req.clone({
    setHeaders: {
      Authorization: authService.getBearerToken(),
      BranchId: String(authService.getBranchId()),
      Role: String(authService.getRoleId()),
      CompanyId: String(authService.getCompanyId()),
      SessionId: generateSessionId(),
    },
  });

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        authService.unAuthorizedLogOut();
      }
      return throwError(() => error);
    })
  );
};

/**
 * Generate unique session ID for request tracking
 */
function generateSessionId(): string {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
    const r = (Math.random() * 16) | 0;
    const v = c === 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}