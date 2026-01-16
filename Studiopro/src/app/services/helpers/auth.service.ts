import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { STORAGE_KEYS, StorageService } from './storage.service';

/**
 * User permission interface
 */
export interface Permission {
  CanCreate: boolean;
  CanRead: boolean;
  CanUpdate: boolean;
  CanDelete: boolean;
  CanPrint: boolean;
  CanExport: boolean;
}

/**
 * Screen interface
 */
export interface Screen {
  ScreenId: number;
  ScreenName: string;
  ScreenPath: string;
  Icon?: string;
}

/**
 * Module interface
 */
export interface Module {
  ModuleId: number;
  ModuleName: string;
  Icon?: string;
  Screens: Screen[];
}

/**
 * User data interface
 */
export interface UserData {
  UserId: number;
  UserName: string;
  Email: string;
  FullName: string;
  ProfileImage?: string;

  // Preferences
  Theme?: 'light' | 'dark' | 'system';

  // Security fields
  IsLocked?: boolean;
  FailedLoginAttempts?: number;
  LockoutEndDate?: string | null;
  PasswordChangedDate?: string | null;
  MustChangePassword?: boolean;
  LoginExpiryDays?: number;

  [key: string]: unknown;
}

/**
 * Modern Auth Service
 * - Uses Angular signals for reactive state
 * - Type-safe methods
 * - Centralized authentication management
 */
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly router = inject(Router);
  private readonly storage = inject(StorageService);
  private readonly messageService = inject(MessageService);

  // Private signals for internal state
  private readonly _isAuthenticated = signal<boolean>(false);
  private readonly _user = signal<UserData | null>(null);
  private readonly _permission = signal<Permission | null>(null);

  // Public readonly computed signals
  readonly isAuthenticated = computed(() => this._isAuthenticated());
  readonly user = computed(() => this._user());
  readonly permission = computed(() => this._permission());
  readonly userName = computed(() => this._user()?.UserName ?? '');
  readonly userId = computed(() => this._user()?.UserId ?? 0);

  private sessionWatcher: ReturnType<typeof setInterval> | null = null;

  constructor() {
    this.initializeFromStorage();
  }

  /**
   * Initialize state from storage on service creation
   */
  private initializeFromStorage(): void {
    const token = this.storage.get<string>(STORAGE_KEYS.AUTH_TOKEN);
    if (token) {
      this._isAuthenticated.set(true);
      this._user.set(this.storage.get<UserData>(STORAGE_KEYS.USER_DATA));
      this._permission.set(this.storage.get<Permission>(STORAGE_KEYS.USER_PERMISSION, 'session'));
      this.startSessionWatcher();
    }
  }

  // ==================== SETTERS ====================

  setToken(token: string): void {
    this.storage.set(STORAGE_KEYS.AUTH_TOKEN, token);
    this._isAuthenticated.set(true);
    this.startSessionWatcher();
  }

  setUserData(userData: UserData): void {
    this.storage.set(STORAGE_KEYS.USER_DATA, userData);
    this._user.set(userData);
  }

  setRoleId(roleId: number | string): void {
    this.storage.set(STORAGE_KEYS.ROLE_ID, roleId, 'session');
  }

  setPlantId(plantId: number | string): void {
    this.storage.set(STORAGE_KEYS.PLANT_ID, plantId, 'session');
  }

  setBranchId(branchId: number | string): void {
    this.storage.set(STORAGE_KEYS.BRANCH_ID, branchId, 'session');
  }

  setCompanyId(companyId: number | string): void {
    this.storage.set(STORAGE_KEYS.COMPANY_ID, companyId, 'session');
  }

  setCompAccYear(year: number | string): void {
    this.storage.set(STORAGE_KEYS.COMPANY_ACC_YEAR, year, 'session');
  }

  setIndustryId(industryId: number | string): void {
    this.storage.set(STORAGE_KEYS.INDUSTRY_ID, industryId, 'session');
  }

  setAppVersion(version: string): void {
    this.storage.set(STORAGE_KEYS.APP_VERSION, version);
  }

  setLoggedInTime(datetime: string): void {
    this.storage.set(STORAGE_KEYS.LOGGED_IN_TIME, datetime);
  }

  setUserModuleList(modules: Module[]): void {
    this.storage.set(STORAGE_KEYS.USER_MODULE_LIST, modules, 'session');
  }

  setUserScreenList(screens: Screen[]): void {
    this.storage.set(STORAGE_KEYS.USER_SCREEN_LIST, screens, 'session');
  }

  setUserPermission(permission: Permission): void {
    this.storage.set(STORAGE_KEYS.USER_PERMISSION, permission, 'session');
    this._permission.set(permission);
  }

  // ==================== GETTERS ====================

  getToken(): string | null {
    return this.storage.get<string>(STORAGE_KEYS.AUTH_TOKEN);
  }

  getBearerToken(): string {
    const token = this.getToken();
    return token ? `Bearer ${token}` : '';
  }

  getUserData(): UserData | null {
    return this.storage.get<UserData>(STORAGE_KEYS.USER_DATA);
  }

  getRoleId(): number {
    return this.storage.get<number>(STORAGE_KEYS.ROLE_ID, 'session') ?? 0;
  }

  getPlantId(): number {
    return this.storage.get<number>(STORAGE_KEYS.PLANT_ID, 'session') ?? 0;
  }

  getBranchId(): number {
    return this.storage.get<number>(STORAGE_KEYS.BRANCH_ID, 'session') ?? 0;
  }

  getCompanyId(): number {
    return this.storage.get<number>(STORAGE_KEYS.COMPANY_ID, 'session') ?? 0;
  }

  getCompAccYear(): number {
    return this.storage.get<number>(STORAGE_KEYS.COMPANY_ACC_YEAR, 'session') ?? 0;
  }

  getIndustryId(): number {
    return this.storage.get<number>(STORAGE_KEYS.INDUSTRY_ID, 'session') ?? 0;
  }

  getAppVersion(): string | null {
    return this.storage.get<string>(STORAGE_KEYS.APP_VERSION);
  }

  getLoggedInTime(): string | null {
    return this.storage.get<string>(STORAGE_KEYS.LOGGED_IN_TIME);
  }

  getUserModuleList(): Module[] {
    return this.storage.get<Module[]>(STORAGE_KEYS.USER_MODULE_LIST, 'session') ?? [];
  }

  getUserScreenList(): Screen[] {
    return this.storage.get<Screen[]>(STORAGE_KEYS.USER_SCREEN_LIST, 'session') ?? [];
  }

  getUserPermission(): Permission | null {
    return this.storage.get<Permission>(STORAGE_KEYS.USER_PERMISSION, 'session');
  }

  // ==================== AUTH CHECKS ====================

  isLoggedIn(): boolean {
    const token = this.getToken();
    return token !== null && token !== '' && token !== undefined;
  }

  hasPermission(action: keyof Permission): boolean {
    const permission = this.getUserPermission();
    return permission ? permission[action] : false;
  }

  // ==================== LOGOUT METHODS ====================

  logout(): void {
    this.stopSessionWatcher();
    this.storage.clearAll();
    this._isAuthenticated.set(false);
    this._user.set(null);
    this._permission.set(null);
    this.router.navigate(['']);
  }

  sessionLogout(): void {
    this.showMessage('error', 'Session Ended', 'Your session was ended. Please login again.');
    this.logout();
  }

  unAuthorizedLogOut(): void {
    this.logout();
    this.showMessage('error', 'Unauthorized', "You're not authorized. Please login.");
  }

  versionUpdateLogout(message?: string): void {
    this.logout();
    if (message) {
      this.showMessage('error', 'Log Out', 'App Version Updated. Please login again.');
    }
  }

  // ==================== NAVIGATION ====================

  redirectToHome(title?: string, message?: string): void {
    this.router.navigate(['/home']);
    if (message) {
      this.showMessage('error', title ?? 'Error', message);
    }
  }

  permissionDenied(title?: string, message?: string): void {
    this.router.navigate(['/home/x403']);
    if (message) {
      this.showMessage('error', title ?? 'Access Denied', message);
    }
  }

  // ==================== PRIVATE METHODS ====================

  private showMessage(severity: string, summary: string, detail: string): void {
    this.messageService.add({
      severity,
      summary,
      detail,
      sticky: true,
      key: 'auth',
    });

    // Auto-clear after 10 seconds
    setTimeout(() => {
      this.messageService.clear('auth');
    }, 10000);
  }

  private startSessionWatcher(): void {
    if (this.sessionWatcher) return;

    this.sessionWatcher = setInterval(() => {
      if (!this.isLoggedIn()) {
        this.logout();
      } else {
        const pathname = location.pathname;
        if (pathname === '' || pathname.includes('login')) {
          this.router.navigate(['/home']);
        }
      }
    }, 2000);
  }

  private stopSessionWatcher(): void {
    if (this.sessionWatcher) {
      clearInterval(this.sessionWatcher);
      this.sessionWatcher = null;
    }
  }
}