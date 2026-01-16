import { computed, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BaseApiService } from '../core/base-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CompanyService extends BaseApiService {

  protected override readonly baseUrl = environment.Company;

  // Service-level state using signals
  private readonly _selectedCompanyId = signal<number>(0);
  private readonly _isViewMode = signal<boolean>(false);

  readonly selectedCompanyId = computed(() => this._selectedCompanyId());
  readonly isViewMode = computed(() => this._isViewMode());
  readonly isEditMode = computed(() => this._selectedCompanyId() > 0 && !this._isViewMode());

  PageInit(params: any = {}): Observable<any> {
    return this.post<any>('PageInit', params, { skipLoading: false });
  }

  Update(params: any): Observable<any> {
    return this.post<any>('Update', params, { skipLoading: false });
  }

  CreateAddress(params: any): Observable<any> {
    return this.post<any>('CreateAddress', params, { skipLoading: false });
  }

  UpdateAddress(params: any): Observable<any> {
    return this.post<any>('UpdateAddress', params, { skipLoading: false });
  }


  ChangeStatus(params: any): Observable<any> {
    return this.post<any>('ChangeStatus', params, { skipLoading: false });
  }


  resetToCreateMode(): void {
    this._selectedCompanyId.set(0);
    this._isViewMode.set(false);
  }
}
