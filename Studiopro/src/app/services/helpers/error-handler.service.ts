import { Injectable, inject, ErrorHandler } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { LoggerService } from './logger.service';
import { AuthService } from './auth.service';

/**
 * Custom API Response interface
 */
export interface CustomResponse<T = unknown> {
  IsSuccess: boolean;
  StatusCode: number;
  Title: string;
  Message: string;
  Data: T;
}

/**
 * Error details interface
 */
export interface ErrorDetails {
  status: number;
  message: string;
  title: string;
  timestamp: Date;
  url?: string;
}

/**
 * Modern Error Handler Service
 * - Centralized error handling
 * - HTTP error mapping
 * - Toast notifications via PrimeNG
 * - Implements Angular ErrorHandler
 */
@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService implements ErrorHandler {
  private readonly messageService = inject(MessageService);
  private readonly logger = inject(LoggerService);
  private readonly authService = inject(AuthService);

  /**
   * Angular ErrorHandler implementation
   */
  handleError(error: Error | HttpErrorResponse | unknown): void {
    this.logger.error('Application error:', error);

    // Do NOT handle HTTP errors here — interceptor already did
    if (error instanceof HttpErrorResponse) {
      return;
    }

    if (error instanceof Error) {
      this.showError('Application Error', error.message);
    } else {
      this.showError('Unknown Error', 'An unexpected error occurred');
    }
  }


  /**
   * Handle HTTP errors
   */
  private handleHttpError(error: HttpErrorResponse): void {
    const errorDetails = this.mapHttpError(error);

    if (error.status === 401) {
      this.authService.unAuthorizedLogOut();
      return;
    }

    this.showError(errorDetails.title, errorDetails.message);
  }

  /**
   * Handle network errors (status 0)
   */
  handleNetworkError(error: HttpErrorResponse): void {
    this.logger.error('Network error:', error);
    this.showError('Network Error', 'Unable to connect to the server. Please check your internet connection.');
  }

  /**
   * Handle client errors (400-499)
   */
  handleClientError(error: HttpErrorResponse): void {
    const details = this.mapHttpError(error);
    this.logger.warn('Client error:', error);

    if (error.status !== 401) {
      // 401 is handled by auth interceptor
      this.showError(details.title, details.message);
    }
  }

  /**
   * Handle server errors (500+)
   */
  handleServerError(error: HttpErrorResponse): void {
    const details = this.mapHttpError(error);
    this.logger.error('Server error:', error);
    this.showError(details.title, details.message);
  }

  /**
   * Map HTTP status codes to user-friendly messages
   */
  private mapHttpError(error: HttpErrorResponse): ErrorDetails {
    const errorMap: Record<number, { title: string; message: string }> = {
      0: { title: 'Network Error', message: 'Unable to connect to the server' },
      400: { title: 'Bad Request', message: error.error?.message || 'Invalid request data' },
      401: { title: 'Unauthorized', message: 'Please login to continue' },
      403: { title: 'Access Denied', message: 'You do not have permission to perform this action' },
      404: { title: 'Not Found', message: 'The requested resource was not found' },
      408: { title: 'Request Timeout', message: 'The request took too long to complete' },
      409: { title: 'Conflict', message: 'A conflict occurred with the current state' },
      422: { title: 'Validation Error', message: error.error?.message || 'Invalid data provided' },
      429: { title: 'Too Many Requests', message: 'Please wait before making another request' },
      500: { title: 'Server Error', message: 'An internal server error occurred' },
      502: { title: 'Bad Gateway', message: 'Server is temporarily unavailable' },
      503: { title: 'Service Unavailable', message: 'Service is temporarily unavailable' },
      504: { title: 'Gateway Timeout', message: 'Server took too long to respond' },
    };

    const mapped = errorMap[error.status] || {
      title: 'Error',
      message: error.message || 'An unexpected error occurred',
    };

    return {
      status: error.status,
      title: mapped.title,
      message: mapped.message,
      timestamp: new Date(),
      url: error.url ?? undefined,
    };
  }

  // ==================== MESSAGE METHODS ====================

  /**
   * Show error toast
   */
  showError(title: string, message: string, life?: number): void {
    this.messageService.add({
      severity: 'error',
      summary: title,
      detail: message,
      life: life ?? 5000,
    });
  }

  /**
   * Show success toast
   */
  showSuccess(title: string, message: string, life?: number): void {
    this.messageService.add({
      severity: 'success',
      summary: title,
      detail: message,
      life: life ?? 3000,
    });
  }

  /**
   * Show warning toast
   */
  showWarning(title: string, message: string, life?: number): void {
    this.messageService.add({
      severity: 'warn',
      summary: title,
      detail: message,
      life: life ?? 5000,
    });
  }

  /**
   * Show info toast
   */
  showInfo(title: string, message: string, life?: number): void {
    this.messageService.add({
      severity: 'info',
      summary: title,
      detail: message,
      life: life ?? 3000,
    });
  }

  // ==================== CUSTOM RESPONSE HANDLERS ====================

  /**
   * Handle custom API error response
   */
  handleCustomError(response: CustomResponse): void {
    if (!response) return;

    this.showError(response.Title, response.Message);
    this.logger.error('API Error:', response);

    if (response.StatusCode === 401) {
      this.authService.unAuthorizedLogOut();
    }
  }

  /**
   * Handle custom API success response
   */
  handleCustomSuccess(response: CustomResponse): void {
    if (!response) return;
    this.showSuccess(response.Title, response.Message);
  }

  /**
   * Handle custom API warning response
   */
  handleCustomWarning(response: CustomResponse): void {
    if (!response) return;
    this.showWarning(response.Title, response.Message);
  }
}