import { HttpClient, HttpContext, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { SKIP_LOADING, SKIP_AUTH } from '../interceptors/interceptor.tokens';

/**
 * Request configuration options
 */
export interface RequestOptions {
  params?: HttpParams | { [param: string]: string | number | boolean | readonly (string | number | boolean)[] };
  headers?: HttpHeaders | { [header: string]: string | string[] };
  skipLoading?: boolean;
  skipAuth?: boolean;
  reportProgress?: boolean;
}

/**
 * Modern Base API Service using Angular 18+ patterns
 * Uses functional injection and supports both typed and 'any' responses
 */
export abstract class BaseApiService {
  protected readonly http = inject(HttpClient);

  /**
   * Abstract property - must be defined in child services
   */
  protected abstract readonly baseUrl: string;

  /**
   * Build full URL from endpoint
   */
  protected buildUrl(endpoint: string): string {
    const base = this.baseUrl.endsWith('/') ? this.baseUrl.slice(0, -1) : this.baseUrl;
    const path = endpoint.startsWith('/') ? endpoint : `/${endpoint}`;
    return `${base}${path}`;
  }

  /**
   * Build HTTP context for interceptors
   */
  protected buildContext(options?: RequestOptions): HttpContext {
    let context = new HttpContext();
    if (options?.skipLoading) {
      context = context.set(SKIP_LOADING, true);
    }
    if (options?.skipAuth) {
      context = context.set(SKIP_AUTH, true);
    }
    return context;
  }

  /**
   * GET request - supports both typed and any response
   */
  protected get<T = any>(endpoint: string, options?: RequestOptions): Observable<T> {
    return this.http.get<T>(this.buildUrl(endpoint), {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
    });
  }

  /**
   * POST request - supports both typed and any response
   */
  protected post<T = any>(endpoint: string, body: any, options?: RequestOptions): Observable<T> {
    return this.http.post<T>(this.buildUrl(endpoint), body, {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
    });
  }

  /**
   * PUT request - supports both typed and any response
   */
  protected put<T = any>(endpoint: string, body: any, options?: RequestOptions): Observable<T> {
    return this.http.put<T>(this.buildUrl(endpoint), body, {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
    });
  }

  /**
   * PATCH request - supports both typed and any response
   */
  protected patch<T = any>(endpoint: string, body: any, options?: RequestOptions): Observable<T> {
    return this.http.patch<T>(this.buildUrl(endpoint), body, {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
    });
  }

  /**
   * DELETE request - supports both typed and any response
   */
  protected delete<T = any>(endpoint: string, options?: RequestOptions): Observable<T> {
    return this.http.delete<T>(this.buildUrl(endpoint), {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
    });
  }

  /**
   * POST request that returns a Blob (for file downloads)
   */
  protected postBlob(endpoint: string, body: any, options?: RequestOptions): Observable<Blob> {
    return this.http.post(this.buildUrl(endpoint), body, {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
      responseType: 'blob',
      reportProgress: options?.reportProgress,
    });
  }

  /**
   * GET request that returns a Blob (for file downloads)
   */
  protected getBlob(endpoint: string, options?: RequestOptions): Observable<Blob> {
    return this.http.get(this.buildUrl(endpoint), {
      params: options?.params,
      headers: options?.headers,
      context: this.buildContext(options),
      responseType: 'blob',
      reportProgress: options?.reportProgress,
    });
  }
}