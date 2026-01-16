import { Injectable, signal, computed, effect } from '@angular/core';
import CryptoJS from 'crypto-js';

/**
 * Encryption key - In production, use environment variable
 */
const ENCRYPTION_KEY = 'YOUR_SECURE_KEY_HERE'; // TODO: Move to environment config

/**
 * Storage type enum
 */
export type StorageType = 'local' | 'session';

/**
 * Modern Storage Service
 * - Unified interface for localStorage and sessionStorage
 * - AES encryption for sensitive data
 * - Reactive signals for state management
 * - Type-safe operations
 */
@Injectable({
  providedIn: 'root',
})
export class StorageService {
  /**
   * Encrypt value using AES
   */
  private encrypt<T>(value: T): string {
    if (value === null || value === undefined) {
      return '';
    }
    return CryptoJS.AES.encrypt(JSON.stringify(value), ENCRYPTION_KEY).toString();
  }

  /**
   * Decrypt value using AES
   */
  private decrypt<T>(encryptedValue: string | null): T | null {
    if (!encryptedValue) {
      return null;
    }
    try {
      const bytes = CryptoJS.AES.decrypt(encryptedValue, ENCRYPTION_KEY);
      const decryptedString = bytes.toString(CryptoJS.enc.Utf8);
      return decryptedString ? JSON.parse(decryptedString) : null;
    } catch (error) {
      console.error('Decryption failed:', error);
      return null;
    }
  }

  /**
   * Get storage instance based on type
   */
  private getStorage(type: StorageType): Storage {
    return type === 'local' ? localStorage : sessionStorage;
  }

  /**
   * Set encrypted value
   */
  set<T>(key: string, value: T, storage: StorageType = 'local'): void {
    const encrypted = this.encrypt(value);
    this.getStorage(storage).setItem(key, encrypted);
  }

  /**
   * Get decrypted value
   */
  get<T>(key: string, storage: StorageType = 'local'): T | null {
    const encrypted = this.getStorage(storage).getItem(key);
    return this.decrypt<T>(encrypted);
  }

  /**
   * Set plain (unencrypted) value
   */
  setPlain<T>(key: string, value: T, storage: StorageType = 'local'): void {
    this.getStorage(storage).setItem(key, JSON.stringify(value));
  }

  /**
   * Get plain (unencrypted) value
   */
  getPlain<T>(key: string, storage: StorageType = 'local'): T | null {
    const value = this.getStorage(storage).getItem(key);
    if (!value) return null;
    try {
      return JSON.parse(value);
    } catch {
      return null;
    }
  }

  /**
   * Remove value from storage
   */
  remove(key: string, storage: StorageType = 'local'): void {
    this.getStorage(storage).removeItem(key);
  }

  /**
   * Clear all values from specific storage
   */
  clear(storage: StorageType = 'local'): void {
    this.getStorage(storage).clear();
  }

  /**
   * Clear all storages
   */
  clearAll(): void {
    localStorage.clear();
    sessionStorage.clear();
  }

  /**
   * Check if key exists
   */
  has(key: string, storage: StorageType = 'local'): boolean {
    return this.getStorage(storage).getItem(key) !== null;
  }

  /**
   * Get all keys from storage
   */
  keys(storage: StorageType = 'local'): string[] {
    const storageInstance = this.getStorage(storage);
    return Object.keys(storageInstance);
  }
}

/**
 * Storage keys constants - prevents typos and enables autocomplete
 */
export const STORAGE_KEYS = {
  AUTH_TOKEN: 'auth_token',
  USER_DATA: 'userData',
  APP_VERSION: 'version',
  LOGGED_IN_TIME: 'LoggedinTime',
  ROLE_ID: 'RoleId',
  PLANT_ID: 'PlantId',
  BRANCH_ID: 'BranchId',
  COMPANY_ID: 'CompanyId',
  COMPANY_ACC_YEAR: 'CompanyAccYear',
  INDUSTRY_ID: 'Industryid',
  USER_MODULE_LIST: 'userModuleList',
  USER_SCREEN_LIST: 'userScreenList',
  USER_PERMISSION: 'userPermission',
  THEME: 'theme',
} as const;