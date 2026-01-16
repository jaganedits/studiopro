import { CommonModule } from '@angular/common';
import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { SelectModule } from 'primeng/select';
import { Table, TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { CRole } from "../c-role/c-role";
import { RoleService } from '../../../../services/api/role-service';
import { ExportService } from '../../../../services/helpers/export.service';
import { animate, style, transition, trigger } from '@angular/animations';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-v-role',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    TagModule,
    CheckboxModule,
    PasswordModule,
    TooltipModule,
    CRole
  ],
  templateUrl: './v-role.html',
  styleUrls: ['./v-role.scss'],
  animations: [
    trigger('pageAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('300ms ease-out',
          style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-in',
          style({ opacity: 0, transform: 'translateY(-20px)' }))
      ])
    ]),
    trigger('dialogAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.95) translateY(-10px)' }),
        animate('200ms ease-out',
          style({ opacity: 1, transform: 'scale(1) translateY(0)' }))
      ]),
      transition(':leave', [
        animate('150ms ease-in',
          style({ opacity: 0, transform: 'scale(0.95) translateY(-10px)' }))
      ])
    ])
  ]
})
export class VRole implements OnInit {
  @ViewChild(CRole) cRoleComponent!: CRole;
  @ViewChild('dt') dt!: Table;

  showDialog: boolean = false;
  dialogHeader: string = 'Add Role';
  selectedRole: any = null;

  RoleList = signal<any[]>([]);

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private roleService: RoleService,
    private exportService: ExportService
  ) { }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    const params = {
      RoleId: 1,
      CompanyId: 1,
      BranchId: 0
    };

    this.roleService.ListInit(params).subscribe({
      next: (res) => {
        this.RoleList.set(res.Data.RoleList || []);
      }

    });
  }

  openAddDialog(): void {
    this.selectedRole = { Action: 'Add' };
    this.dialogHeader = 'Add Role';
    this.showDialog = true;
  }

  edit(role: any): void {
    this.selectedRole = { ...role, Action: 'Edit' };
    this.dialogHeader = 'Edit Role';
    this.showDialog = true;
  }

  view(role: any): void {
    this.selectedRole = { ...role, Action: 'View' };
    this.dialogHeader = 'View Role';
    this.showDialog = true;
  }

  delete(role: any): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${role.RoleName}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.roleService.ChangeStatus({ RoleId: role.RoleId, UserId: 1 }).subscribe({
          next: () => {

            this.loadRoles();
          }
        });
      }
    });
  }

  // Save role handler
  saveRole(): void {
    if (this.cRoleComponent) {
      this.cRoleComponent.saveRole();
    }
  }

  // Handle save success from child component
  onSaveSuccess(event: any): void {

    this.showDialog = false;
    this.loadRoles();
  }

  // Handle close dialog
  onCloseDialog(): void {
    this.showDialog = false;
  }

  // Get filtered or all data for export
  private getExportData() {
    const dataToExport = this.dt.filteredValue ?? this.RoleList();
    return dataToExport.map(role => ({
      'Code': role.RoleCode,
      'Role Name': role.RoleName,
      'Status': role.StatusName
    }));
  }

  // Export to Excel
  exportToExcel(): void {
    const exportData = this.getExportData();
    this.exportService.export(exportData, {
      filename: 'Roles',
      format: 'excel',
      styled: true,
      title: 'Role List',
      headerColor: '7C3AED'
    });
  }

  // Export to PDF
  exportToPdf(): void {
    const exportData = this.getExportData();
    this.exportService.export(exportData, {
      filename: 'Roles',
      format: 'pdf',
      title: 'Role List',
      headerColor: '7C3AED'
    });
  }
}