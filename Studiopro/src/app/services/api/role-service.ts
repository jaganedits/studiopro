import { computed, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BaseApiService } from '../core/base-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RoleService extends BaseApiService {

  protected override readonly baseUrl = environment.Role;

  // Service-level state using signals
  private readonly _selectedRoleId = signal<number>(0);
  private readonly _isViewMode = signal<boolean>(false);

  readonly selectedRoleId = computed(() => this._selectedRoleId());
  readonly isViewMode = computed(() => this._isViewMode());
  readonly isEditMode = computed(() => this._selectedRoleId() > 0 && !this._isViewMode());
  readonly isCreateMode = computed(() => this._selectedRoleId() === 0 && !this._isViewMode());



  ListInit(params: any): Observable<any> {
    this.resetToCreateMode();
    return this.post<any>('ListInit', params, { skipLoading: false });
  }

  PageInit(params: any = {}): Observable<any> {
    return this.post<any>('PageInit', params, { skipLoading: false });
  }

  Create(params: any): Observable<any> {
    this.resetToCreateMode();
    return this.post<any>('Create', params, { skipLoading: false });
  }
  Update(params: any): Observable<any> {
    return this.post<any>('Update', params, { skipLoading: false });
  }

  ChangeStatus(params: any): Observable<any> {
    return this.post<any>('ChangeStatus', params, { skipLoading: false });
  }
  resetToCreateMode(): void {
    this._selectedRoleId.set(0);
    this._isViewMode.set(false);
  }


}
