import { Injectable, inject } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { Observable, Subject } from 'rxjs';

/**
 * Confirmation result type
 */
export type ConfirmResult = 'accept' | 'reject';

/**
 * Confirmation options interface
 */
export interface ConfirmOptions {
  header: string;
  message: string;
  icon?: string;
  acceptLabel?: string;
  rejectLabel?: string;
  acceptButtonStyleClass?: string;
  rejectButtonStyleClass?: string;
  acceptIcon?: string;
  rejectIcon?: string;
  blockScroll?: boolean;
  closeOnEscape?: boolean;
  dismissableMask?: boolean;
}

/**
 * Modern Confirm Service
 * - Promise-based confirmation dialogs
 * - Observable-based alternative
 * - Pre-built common confirmation patterns
 */
@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  private readonly confirmationService = inject(ConfirmationService);

  /**
   * Generic confirmation dialog (Promise-based)
   */
  confirm(options: ConfirmOptions): Promise<boolean> {
    return new Promise((resolve) => {
      this.confirmationService.confirm({
        ...options,
        accept: () => resolve(true),
        reject: () => resolve(false),
      });
    });
  }

  /**
   * Generic confirmation dialog (Observable-based)
   */
  confirm$(options: ConfirmOptions): Observable<boolean> {
    const result$ = new Subject<boolean>();

    this.confirmationService.confirm({
      ...options,
      accept: () => {
        result$.next(true);
        result$.complete();
      },
      reject: () => {
        result$.next(false);
        result$.complete();
      },
    });

    return result$.asObservable();
  }

  /**
   * Callback-based confirmation (legacy support)
   */
  confirmWithCallback(
    header: string,
    message: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header,
      message,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  // ==================== PRE-BUILT CONFIRMATIONS ====================

  /**
   * Delete confirmation
   */
  async delete(recordName: string): Promise<boolean> {
    return this.confirm({
      header: 'Delete Confirmation',
      message: `Are you sure you want to delete <strong>'${recordName}'</strong>?`,
      icon: 'pi pi-trash',
      acceptLabel: 'Delete',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-danger',
    });
  }

  /**
   * Delete confirmation with callback (legacy)
   */
  deleteConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Delete Confirmation',
      message: `Are you sure you want to delete this record <strong>'${recordName}'</strong>?`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Clear/Reset confirmation
   */
  async clear(recordName: string): Promise<boolean> {
    return this.confirm({
      header: 'Clear Confirmation',
      message: `Are you sure you want to clear <strong>'${recordName}'</strong>?`,
      icon: 'pi pi-eraser',
      acceptLabel: 'Clear',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-warning',
    });
  }

  /**
   * Clear confirmation with callback (legacy)
   */
  clearConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Clear Confirmation',
      message: `Are you sure you want to clear this record <strong>'${recordName}'</strong>?`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Inactive/Deactivate confirmation
   */
  async inactive(recordName: string, showWarning = false): Promise<boolean> {
    const baseMessage = `Are you sure you want to deactivate <strong>'${recordName}'</strong>?`;
    const warningMessage = showWarning
      ? `${baseMessage}<br/>This may affect other screens and records.`
      : baseMessage;

    return this.confirm({
      header: 'Deactivate Confirmation',
      message: warningMessage,
      icon: 'pi pi-ban',
      acceptLabel: 'Deactivate',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-warning',
    });
  }

  /**
   * Inactive confirmation with callback (legacy)
   */
  inactiveConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Inactive Confirmation',
      message: `Are you sure you want to in-active this record <strong>'${recordName}'</strong>?`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Inactive with warning confirmation (legacy)
   */
  inactiveWarning(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Inactive Confirmation',
      message: `Are you sure you want to in-active this record <strong>'${recordName}'</strong>? <br/> It might affect other screens and records.`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Cancel confirmation
   */
  async cancel(recordName: string): Promise<boolean> {
    return this.confirm({
      header: 'Cancel Confirmation',
      message: `Are you sure you want to cancel <strong>'${recordName}'</strong>?`,
      icon: 'pi pi-times-circle',
      acceptLabel: 'Yes, Cancel',
      rejectLabel: 'No',
      acceptButtonStyleClass: 'p-button-danger',
    });
  }

  /**
   * Cancel confirmation with callback (legacy)
   */
  cancelConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Cancel Confirmation',
      message: `Are you sure you want to cancel this record <strong>'${recordName}'</strong>?`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Approval confirmation
   */
  async approve(recordName: string): Promise<boolean> {
    return this.confirm({
      header: 'Approval Confirmation',
      message: `Are you sure you want to approve <strong>'${recordName}'</strong>?`,
      icon: 'pi pi-check-circle',
      acceptLabel: 'Approve',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-success',
    });
  }

  /**
   * Approval confirmation with callback (legacy)
   */
  approvalConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Approval Confirmation',
      message: `Are you sure you want to approve this record ${recordName}?`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Upload/Replace confirmation
   */
  async upload(): Promise<boolean> {
    return this.confirm({
      header: 'Upload Confirmation',
      message: 'Are you sure you want to replace existing records?',
      icon: 'pi pi-upload',
      acceptLabel: 'Replace',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-warning',
    });
  }

  /**
   * Upload confirmation with callback (legacy)
   */
  uploadConfirm(
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Upload Confirmation',
      message: 'Are you sure you want to replace records?',
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }

  /**
   * Save confirmation
   */
  async save(): Promise<boolean> {
    return this.confirm({
      header: 'Save Confirmation',
      message: 'Are you sure you want to save the changes?',
      icon: 'pi pi-save',
      acceptLabel: 'Save',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-success',
    });
  }

  /**
   * Submit confirmation
   */
  async submit(action = 'submit'): Promise<boolean> {
    return this.confirm({
      header: 'Submit Confirmation',
      message: `Are you sure you want to ${action}?`,
      icon: 'pi pi-send',
      acceptLabel: 'Submit',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-primary',
    });
  }

  /**
   * Custom danger confirmation
   */
  async danger(header: string, message: string): Promise<boolean> {
    return this.confirm({
      header,
      message,
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Proceed',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-danger',
    });
  }

  /**
   * Delete package name confirmation (legacy)
   */
  deletePackageNameConfirm(
    recordName: string,
    onAccept: () => void,
    onReject?: () => void
  ): void {
    this.confirmationService.confirm({
      header: 'Delete Confirmation',
      message: `Are you sure you want to delete this record <strong>'${recordName}'</strong>? It'll reflect on all the Markup levels for this particular Package Name.`,
      accept: onAccept,
      reject: onReject ?? (() => {}),
    });
  }
}