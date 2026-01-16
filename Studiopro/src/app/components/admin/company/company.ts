import { Component, inject, OnInit, signal, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { MessageModule } from 'primeng/message';
import { CompanyService } from '../../../services/api/company-service';
import { ErrorHandlerService } from '../../../services/helpers';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    ToastModule,
    ConfirmDialogModule,
    TableModule,
    DialogModule,
    CheckboxModule,
    SelectModule,
    TagModule,
    TooltipModule,
    MessageModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './company.html',
  styleUrl: './company.scss',
})
export class Company implements OnInit {
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private companyService = inject(CompanyService);
  private errorHandlerService = inject(ErrorHandlerService);

  @ViewChild('logoInput') logoInput!: ElementRef<HTMLInputElement>;
  @ViewChild('signatureInput') signatureInput!: ElementRef<HTMLInputElement>;

  activeTab = signal<'general' | 'address'>('general');

  // Forms
  companyForm!: FormGroup;
  addressForm!: FormGroup;

  // Logo & Signature Files
  logoFile = signal<File | null>(null);
  signatureFile = signal<File | null>(null);

  // Logo & Signature Preview
  logo = signal<string | null>(null);
  signature = signal<string | null>(null);

  // Address Management
  addresses = signal<any[]>([]);
  statusOptions = signal<any[]>([]);

  // Dialog state
  showAddressDialog = signal(false);
  editingAddress = signal<any>(null);
  dialogHeader = signal('Add New Address');

  // States
  isSaving = signal(false);
  submitted = false;
  addressSubmitted = false;

  // Current User ID (should come from auth service)
  private currentUserId = 1;

  ngOnInit(): void {
    this.initializeCompanyForm();
    this.initializeAddressForm();
    this.loadPageData();
  }

  initializeCompanyForm(): void {
    this.companyForm = this.fb.group({
      CompanyId: [0],
      CompanyName: [null, [Validators.required, Validators.maxLength(200)]],
      Tagline: [null, [Validators.maxLength(500)]],
      Phone: [null, [Validators.required]],
      AlternatePhone: [null],
      Email: [null, [Validators.email, Validators.maxLength(100)]],
      Website: [null, [Validators.maxLength(200)]],
      Facebook: [null, [Validators.maxLength(200)]],
      Instagram: [null, [Validators.maxLength(200)]],
      Youtube: [null, [Validators.maxLength(200)]],
      Whatsapp: [null, [Validators.maxLength(200)]],
      GstNumber: [null, [Validators.maxLength(20)]],
      PanNumber: [null, [Validators.maxLength(15)]],
      CinNumber: [null, [Validators.maxLength(25)]],
      BankName: [null, [Validators.maxLength(100)]],
      AccountNumber: [null, [Validators.maxLength(30)]],
      AccountHolderName: [null, [Validators.maxLength(200)]],
      IfscCode: [null, [Validators.maxLength(15)]],
      BranchName: [null, [Validators.maxLength(100)]],
      UpiId: [null, [Validators.maxLength(100)]],
      InvoicePrefix: ['INV', [Validators.maxLength(10)]],
      InvoiceStartNumber: [1001],
      TermsConditions: [null],
      FooterNote: [null, [Validators.maxLength(500)]],
      LogonName: [null, [Validators.maxLength(100)]],
      LogoPath: [null],
      SignaturenName: [null, [Validators.maxLength(100)]],
      SignaturePath: [null],
      Status: [1, Validators.required],
      CreatedBy: [this.currentUserId],
      CreatedDate: [new Date()],
      ModifiedBy: [this.currentUserId],
      ModifiedDate: [new Date()]
    });
  }

  initializeAddressForm(): void {
    this.addressForm = this.fb.group({
      CompanyAddressId: [0],
      CompanyId: [0],
      Label: [null, [Validators.required, Validators.maxLength(100)]],
      Address: [null, [Validators.required, Validators.maxLength(500)]],
      Area: [null, [Validators.maxLength(200)]],
      City: [null, [Validators.required, Validators.maxLength(100)]],
      State: [null, [Validators.required, Validators.maxLength(100)]],
      Pincode: [null, [Validators.required, Validators.maxLength(10)]],
      Landmark: [null, [Validators.maxLength(200)]],
      IsPrimary: [false],
      Status: [1, Validators.required],
      CreatedBy: [this.currentUserId],
      CreatedDate: [new Date()],
      ModifiedBy: [null],
      ModifiedDate: [null]
    });
  }

  loadPageData(): void {
    const params = { CompanyId: 2 };
    this.companyService.PageInit(params).subscribe({
      next: (res) => {
        this.statusOptions.set(res.Data.StatusList || []);
        this.addresses.set(res.Data.AddressList || []);

        if (res.Data.Company) {
          this.companyForm.patchValue(res.Data.Company);
          if (res.Data.Company.LogoPath) {
            this.logo.set(res.Data.Company.LogoPath);
          }
          if (res.Data.Company.SignaturePath) {
            this.signature.set(res.Data.Company.SignaturePath);
          }
        }
      },
      error: (error) => {
        this.errorHandlerService.handleError(error);
      }
    });
  }

  setActiveTab(tab: 'general' | 'address') {
    this.activeTab.set(tab);
  }

  // Validation helpers
  isFieldInvalid(fieldName: string): boolean {
    const field = this.companyForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched || this.submitted) : false;
  }

  getFieldError(fieldName: string): string {
    const errors = this.companyForm.get(fieldName)?.errors;
    if (!errors) return '';

    if (errors['required']) return `${fieldName} is required`;
    if (errors['email']) return 'Invalid email format';
    if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters allowed`;

    return 'Invalid value';
  }

  isAddressFieldInvalid(fieldName: string): boolean {
    const field = this.addressForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched || this.addressSubmitted) : false;
  }

  getAddressFieldError(fieldName: string): string {
    const errors = this.addressForm.get(fieldName)?.errors;
    if (!errors) return '';

    if (errors['required']) return `${fieldName} is required`;
    if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters allowed`;

    return 'Invalid value';
  }

  saveSettings(): void {
    this.submitted = true;
    this.companyForm.markAllAsTouched();

    if (this.companyForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill all required fields',
        life: 3000
      });
      return;
    }

    this.isSaving.set(true);

    // Update modified fields
    this.companyForm.patchValue({
      ModifiedBy: this.currentUserId,
      ModifiedDate: new Date()
    });

    // Build FormData for multipart/form-data request
    const formData = new FormData();
    formData.append('data', JSON.stringify({ Company: this.companyForm.getRawValue() }));

    // Append logo file if selected
    const logoFile = this.logoFile();
    if (logoFile) {
      formData.append('logo', logoFile);
    }

    // Append signature file if selected
    const signatureFile = this.signatureFile();
    if (signatureFile) {
      formData.append('signature', signatureFile);
    }

    this.companyService.Update(formData).subscribe({
      next: () => {
        this.isSaving.set(false);
        // Clear file signals after successful upload
        this.logoFile.set(null);
        this.signatureFile.set(null);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Company settings saved successfully',
          life: 3000
        });
        // Reload to get updated paths
        this.loadPageData();
      },
      error: (error) => {
        this.isSaving.set(false);
        this.errorHandlerService.handleError(error);
      }
    });
  }

  // Address Management Methods
  openAddressDialog(address?: any): void {
    this.addressSubmitted = false;
    if (address) {
      this.editingAddress.set(address);
      this.dialogHeader.set('Edit Address');
      this.addressForm.patchValue(address);
    } else {
      this.editingAddress.set(null);
      this.dialogHeader.set('Add New Address');
      this.addressForm.reset({
        CompanyAddressId: 0,
        CompanyId: this.companyForm.get('CompanyId')?.value || 1,
        Label: null,
        Address: null,
        Area: null,
        City: null,
        State: null,
        Pincode: null,
        Landmark: null,
        IsPrimary: false,
        Status: 1,
        CreatedBy: this.currentUserId,
        CreatedDate: new Date(),
        ModifiedBy: null,
        ModifiedDate: null
      });
    }
    this.showAddressDialog.set(true);
  }

  closeAddressDialog(): void {
    this.showAddressDialog.set(false);
    this.editingAddress.set(null);
    this.addressSubmitted = false;
  }

  saveAddress(): void {
    this.addressSubmitted = true;
    this.addressForm.markAllAsTouched();

    if (this.addressForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill all required fields',
        life: 3000
      });
      return;
    }

    // Update ModifiedBy for edit
    if (this.editingAddress() !== null) {
      this.addressForm.patchValue({
        ModifiedBy: this.currentUserId,
        ModifiedDate: new Date()
      });
    }

    // Backend expects "Address" key, not "CompanyAddress"
    const formData = { Address: this.addressForm.getRawValue() };
    const isEdit = this.editingAddress() !== null;

    const saveObservable = isEdit
      ? this.companyService.UpdateAddress(formData)
      : this.companyService.CreateAddress(formData);

    saveObservable.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Address updated successfully' : 'Address added successfully',
          life: 3000
        });
        this.closeAddressDialog();
        this.loadPageData();
      },
      error: (error) => {
        this.errorHandlerService.handleError(error);
      }
    });
  }

  deleteAddress(address: any): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${address.Label}"?`,
      header: 'Delete Address',
      icon: 'pi pi-trash',
      accept: () => {
        const params = {
          CompanyAddressId: address.CompanyAddressId,
          UserId: this.currentUserId
        };
        this.companyService.ChangeStatus(params).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Address deleted successfully',
              life: 3000
            });
            this.loadPageData();
          },
          error: (error) => {
            this.errorHandlerService.handleError(error);
          }
        });
      }
    });
  }

  getFullAddress(address: any): string {
    const parts = [address.Address, address.Area, address.City, address.State, address.Pincode].filter(Boolean);
    return parts.join(', ');
  }

  // Logo Methods
  triggerLogoUpload(): void {
    this.logoInput.nativeElement.click();
  }

  onLogoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];

      if (!file.type.startsWith('image/')) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Please select an image file'
        });
        return;
      }

      if (file.size > 2 * 1024 * 1024) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Image size should be less than 2MB'
        });
        return;
      }

      // Store file for upload
      this.logoFile.set(file);

      // Preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.logo.set(e.target?.result as string);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Logo selected. Click Save Changes to upload.'
        });
      };
      reader.readAsDataURL(file);
    }
  }

  removeLogo(): void {
    this.confirmationService.confirm({
      message: 'Are you sure you want to remove the logo?',
      header: 'Remove Logo',
      icon: 'pi pi-trash',
      accept: () => {
        this.logo.set(null);
        this.logoFile.set(null);
        if (this.logoInput) {
          this.logoInput.nativeElement.value = '';
        }
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Logo removed successfully'
        });
      }
    });
  }

  // Signature Methods
  triggerSignatureUpload(): void {
    this.signatureInput.nativeElement.click();
  }

  onSignatureSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];

      if (!file.type.startsWith('image/')) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Please select an image file'
        });
        return;
      }

      if (file.size > 2 * 1024 * 1024) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Image size should be less than 2MB'
        });
        return;
      }

      // Store file for upload
      this.signatureFile.set(file);

      // Preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.signature.set(e.target?.result as string);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Signature selected. Click Save Changes to upload.'
        });
      };
      reader.readAsDataURL(file);
    }
  }

  removeSignature(): void {
    this.confirmationService.confirm({
      message: 'Are you sure you want to remove the signature?',
      header: 'Remove Signature',
      icon: 'pi pi-trash',
      accept: () => {
        this.signature.set(null);
        this.signatureFile.set(null);
        if (this.signatureInput) {
          this.signatureInput.nativeElement.value = '';
        }
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Signature removed successfully'
        });
      }
    });
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      minimumFractionDigits: 0
    }).format(value);
  }
}
