import { HttpContextToken } from '@angular/common/http';

/**
 * Token to skip loading indicator for specific requests
 * Usage: context.set(SKIP_LOADING, true)
 */
export const SKIP_LOADING = new HttpContextToken<boolean>(() => false);

/**
 * Token to skip authentication header injection
 * Usage: context.set(SKIP_AUTH, true)
 */
export const SKIP_AUTH = new HttpContextToken<boolean>(() => false);

/**
 * Token to skip error handling (let component handle errors)
 * Usage: context.set(SKIP_ERROR_HANDLING, true)
 */
export const SKIP_ERROR_HANDLING = new HttpContextToken<boolean>(() => false);

/**
 * Token to set custom retry count for specific requests
 * Usage: context.set(RETRY_COUNT, 3)
 */
export const RETRY_COUNT = new HttpContextToken<number>(() => 1);

/**
 * Token to mark request as cacheable
 * Usage: context.set(CACHE_REQUEST, true)
 */
export const CACHE_REQUEST = new HttpContextToken<boolean>(() => false);

/**
 * Token to set cache TTL in milliseconds
 * Usage: context.set(CACHE_TTL, 60000)
 */
export const CACHE_TTL = new HttpContextToken<number>(() => 300000); // 5 minutes default