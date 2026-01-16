import { computed, Injectable, signal } from '@angular/core';
import { BaseApiService } from '../core/base-api.service';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BranchService extends BaseApiService {
  
  protected override readonly baseUrl = environment.Branch;

  // Service-level state using signals
  private readonly _selectedBranchId = signal<number>(0);
  private readonly _isViewMode = signal<boolean>(false);

  readonly selectedBranchId = computed(() => this._selectedBranchId());
  readonly isViewMode = computed(() => this._isViewMode());
  readonly isEditMode = computed(() => this._selectedBranchId() > 0 && !this._isViewMode());
  readonly isCreateMode = computed(() => this._selectedBranchId() === 0 && !this._isViewMode());

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
    this._selectedBranchId.set(0);
    this._isViewMode.set(false);
  }

  setViewMode(branchId: number): void {
    this._selectedBranchId.set(branchId);
    this._isViewMode.set(true);
  }

  setEditMode(branchId: number): void {
    this._selectedBranchId.set(branchId);
    this._isViewMode.set(false);
  }
}
