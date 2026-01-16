import { Injectable, signal, computed } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { BaseApiService } from '../core/base-api.service';
import { environment } from '../../../environments/environment';


/**
 * Modern Users Service
 * - Extends BaseApiService for common functionality
 * - Uses signals for reactive state
 * - Uses 'any' type for flexibility when interfaces are not defined
 */
@Injectable({
    providedIn: 'root',
})
export class UsersService extends BaseApiService {
    protected override readonly baseUrl = environment.User;

    // Service-level state using signals
    private readonly _selectedUserId = signal<number>(0);
    private readonly _isViewMode = signal<boolean>(false);

    // Public computed signals
    readonly selectedUserId = computed(() => this._selectedUserId());
    readonly isViewMode = computed(() => this._isViewMode());
    readonly isEditMode = computed(() => this._selectedUserId() > 0 && !this._isViewMode());
    readonly isCreateMode = computed(() => this._selectedUserId() === 0 && !this._isViewMode());

    // ==================== STATE MANAGEMENT ====================

    /**
     * Set user ID for edit mode
     */
    setEditUser(userId: number): void {
        this._selectedUserId.set(userId);
        this._isViewMode.set(false);
    }

    /**
     * Set user ID for view mode
     */
    setViewUser(userId: number): void {
        this._selectedUserId.set(userId);
        this._isViewMode.set(true);
    }

    /**
     * Reset to create mode
     */
    resetToCreateMode(): void {
        this._selectedUserId.set(0);
        this._isViewMode.set(false);
    }

    /**
     * Check if in update mode
     */
    isUpdate(): boolean {
        return this._selectedUserId() > 0;
    }

    /**
     * Check if in view or update mode
     */
    isViewOrUpdate(): boolean {
        return this.isUpdate() || this._isViewMode();
    }

    // ==================== API METHODS ====================

    /**
     * Initialize search screen data
     */
    searchInitialize(params: any): Observable<any> {
        this.resetToCreateMode();
        return this.post<any>('SearchInitialize', params, { skipLoading: false });
    }

    /**
     * Initialize create screen data
     */
    createInitialize(params: any = {}): Observable<any> {
        return this.post<any>('CreateInitialize', params, { skipLoading: false });
    }

    /**
     * Get employee details
     */
    getEmployee(params: any): Observable<any> {
        return this.post<any>('GetEmployee', params);
    }

    /**
     * Get department and designation lists
     */
    getDepartmentAndDesignation(params: any = {}): Observable<any> {
        return this.post<any>('GetDepartmentandDesignation', params);
    }

    /**
     * Create new user
     */
    create(payload: any): Observable<any> {
        return this.post<any>('Create', payload).pipe(
            tap((response: any) => {
                if (response?.IsSuccess) {
                    this.resetToCreateMode();
                }
            })
        );
    }

    /**
     * Update existing user
     */
    update(payload: any): Observable<any> {
        return this.post<any>('Update', payload).pipe(
            tap((response: any) => {
                if (response?.IsSuccess) {
                    this.resetToCreateMode();
                }
            })
        );
    }

    /**
     * Create or update user (convenience method)
     */
    save(payload: any): Observable<any> {
        return payload.UserId && payload.UserId > 0
            ? this.update(payload)
            : this.create(payload);
    }

    /**
     * Change user status (active/inactive)
     */
    changeStatus(params: any): Observable<any> {
        return this.post<any>('ChangeStatus', params);
    }

    /**
     * Get user by ID
     */
    getById(userId: number): Observable<any> {
        return this.post<any>('GetById', { userId });
    }

    /**
     * Delete user
     */
    deleteUser(userId: number): Observable<any> {
        return this.post<any>('Delete', { userId });
    }

    /**
     * Check if username exists
     */
    checkUsernameExists(username: string, excludeUserId?: number): Observable<any> {
        return this.post<any>('CheckUsernameExists', { username, excludeUserId });
    }

    /**
     * Check if email exists
     */
    checkEmailExists(email: string, excludeUserId?: number): Observable<any> {
        return this.post<any>('CheckEmailExists', { email, excludeUserId });
    }

    /**
     * Reset user password
     */
    resetPassword(params: any): Observable<any> {
        return this.post<any>('ResetPassword', params);
    }

    /**
     * Upload profile image
     */
    uploadProfileImage(userId: number, file: File): Observable<any> {
        const formData = new FormData();
        formData.append('userId', userId.toString());
        formData.append('file', file);
        return this.post<any>('UploadProfileImage', formData);
    }
}