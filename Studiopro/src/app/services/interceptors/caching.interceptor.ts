import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpResponse } from '@angular/common/http';
import { of, tap } from 'rxjs';
import { CACHE_REQUEST, CACHE_TTL } from './interceptor.tokens';

/**
 * Simple in-memory cache storage
 */
interface CacheEntry {
  response: HttpResponse<unknown>;
  expiry: number;
}

const cache = new Map<string, CacheEntry>();

/**
 * Caching Interceptor
 * - Caches GET requests based on URL
 * - Configurable TTL via context token
 * - Only caches when explicitly requested
 */
export const cachingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) => {
  // Only cache GET requests when explicitly requested
  if (req.method !== 'GET' || !req.context.get(CACHE_REQUEST)) {
    return next(req);
  }

  const cacheKey = req.urlWithParams;
  const cachedEntry = cache.get(cacheKey);

  // Return cached response if valid
  if (cachedEntry && cachedEntry.expiry > Date.now()) {
    console.log(`[Cache] HIT: ${cacheKey}`);
    return of(cachedEntry.response.clone());
  }

  // Clear expired entry
  if (cachedEntry) {
    cache.delete(cacheKey);
  }

  const ttl = req.context.get(CACHE_TTL);

  return next(req).pipe(
    tap((event) => {
      if (event instanceof HttpResponse) {
        console.log(`[Cache] STORE: ${cacheKey} (TTL: ${ttl}ms)`);
        cache.set(cacheKey, {
          response: event.clone(),
          expiry: Date.now() + ttl,
        });
      }
    })
  );
};

/**
 * Utility function to clear cache
 */
export function clearCache(): void {
  cache.clear();
  console.log('[Cache] Cleared all entries');
}

/**
 * Utility function to clear specific cache entry
 */
export function clearCacheEntry(url: string): void {
  cache.delete(url);
  console.log(`[Cache] Cleared entry: ${url}`);
}