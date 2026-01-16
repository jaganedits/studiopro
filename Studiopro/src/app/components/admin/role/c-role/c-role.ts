import { Component, OnInit, signal, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { RoleService } from '../../../../services/api/role-service';
import { ErrorHandlerService } from '../../../../services/helpers';

// PrimeNG imports
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-c-role',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SelectModule,
    InputTextModule,
    MessageModule,  // Changed from Message to MessageModule
    ToastModule,
    ButtonModule
  ],
  templateUrl: './c-role.html',
  styleUrls: ['./c-role.scss'],
})
export class CRole implements OnInit {
  @Output() saveSuccess = new EventEmitter<any>();
  @Output() saveError = new EventEmitter<any>();
  @Output() formValid = new EventEmitter<boolean>();
  @Output() closeDialog = new EventEmitter<void>(); // Added this event emitter

  @Input() roleData: any = null; // For edit mode

  statusOptions = signal<any[]>([]);
  roleForm!: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private roleService: RoleService,
    private errorHandlerService: ErrorHandlerService
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    if (this.roleData) {
      this.loadStatusOptions();
    }
  }

  initializeForm(): void {
    this.roleForm = this.fb.group({
      RoleId: [0],
      CompanyId: [1],
      RoleCode: [null],
      RoleName: [null, [
        Validators.required,
        Validators.maxLength(100)
      ]],
      Status: [1, Validators.required],
      CreatedBy: [1],
      CreatedDate: [new Date()],
      ModifiedBy: [1],
      ModifiedDate: [new Date()]
    });
  }

  loadStatusOptions(): void {
    if (!this.roleData) return;

    if (this.roleData.Action === 'View') {
      this.roleForm.disable();
    } else if (this.roleData.Action === 'Edit') {
      this.roleForm.get('RoleCode')?.disable();
    }

    const params = {
      RoleId: this.roleData.Action === 'Add' ? 0 : this.roleData.RoleId,
      CompanyId: this.roleData.Action === 'Add' ? 0 : this.roleData.CompanyId,
      BranchId: 0
    };

    this.roleService.PageInit(params).subscribe({
      next: (res) => {
        this.statusOptions.set(res.Data.StatusList || []);

        // Patch form values based on roleData
        if (this.roleData && this.roleData.Action !== 'Add') {
          this.roleForm.patchValue(res.Data.role);
        }
      },
      error: (error) => {
        this.errorHandlerService.handleError(error);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.roleForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched || this.submitted) : false;
  }

  getFieldError(fieldName: string): string {
    const errors = this.roleForm.get(fieldName)?.errors;
    if (!errors) return '';

    if (errors['required']) return `${fieldName} is required`;
    if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters required`;
    if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters allowed`;
    if (errors['pattern']) return 'Invalid format';

    return 'Invalid value';
  }

  // Method to save role (called from parent)
  saveRole(): void {
    this.submitted = true;
    this.roleForm.markAllAsTouched();

    if (this.roleForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill all required fields',
        life: 3000
      });
      return;
    }

    const formData = { Role: this.roleForm.getRawValue() };

    const saveObservable = this.roleForm.value.RoleId === 0
      ? this.roleService.Create(formData)
      : this.roleService.Update(formData);

    saveObservable.subscribe({
      next: (response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: this.roleForm.value.RoleId === 0 ? 'Role created successfully' : 'Role updated successfully',
          life: 3000
        });

        // Emit success event with response data
        this.saveSuccess.emit({
          success: true,
          data: response,
          isEdit: this.roleForm.value.RoleId !== 0
        });
      },
      error: (error) => {
        this.errorHandlerService.handleError(error);
        this.saveError.emit({
          success: false,
          error: error
        });
      }
    });
  }

  close(): void {
    this.closeDialog.emit();
  }
}