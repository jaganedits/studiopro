import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Table, TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService, ConfirmationService } from 'primeng/api';
import { BranchService } from '../../../../services/api/branch-service';
import { ExportService } from '../../../../services/helpers/export.service';

@Component({
  selector: 'app-v-branchs',
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    ToastModule,
    ConfirmDialogModule,
    TagModule,
    TooltipModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './v-branchs.html',
  styleUrl: './v-branchs.scss',
})
export class VBranchs implements OnInit {
  @ViewChild('dt') dt!: Table;

  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private router = inject(Router);
  private branchService = inject(BranchService);
  private exportService = inject(ExportService);

  branches = signal<BranchListInit[]>([]);
  searchValue = '';


  ngOnInit(): void {
    this.loadBranch()
  }

  loadBranch() {
    const params = {
      CompanyId: 1,
    };

    this.branchService.ListInit(params).subscribe({
      next: (res) => {
        this.branches.set(res.Data.BranchListInitList || []);
      }

    });
  }

  openAddDialog() {
    this.router.navigate(['home/c-branchs']);
  }

  viewBranch(branch: any) {
    this.router.navigate(['home/c-branchs'], { queryParams: { id: branch.BranchId, mode: 'view' } });
  }

  editBranch(branch: any) {
    this.router.navigate(['home/c-branchs'], { queryParams: { id: branch.BranchId, mode: 'edit' } });
  }

  deleteBranch(branch: BranchListInit) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this branch?',
      header: 'Delete Branch',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.branchService.ChangeStatus({ BranchId: branch.BranchId, UserId: 1 }).subscribe({
          next: () => {
            this.loadBranch()

          }
        });
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Branch deleted successfully'
        });
      }
    });
  }


  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.dt.filterGlobal(value, 'contains');
  }

  clearSearch() {
    this.searchValue = '';
    this.dt.filterGlobal('', 'contains');
  }

  // Get filtered or all data for export
  private getExportData() {
    const dataToExport = this.dt.filteredValue ?? this.branches();
    return dataToExport.map(branch => ({
      'Code': branch.BranchCode,
      'Name': branch.BranchName,
      'Address': branch.Address ?? '',
      'Phone': branch.Phone ?? '',
      'Manager': branch.ManagerName ?? '',
      'Status': branch.StatusName
    }));
  }

  exportToExcel() {
    const exportData = this.getExportData();
    this.exportService.export(exportData, {
      filename: 'Branches',
      format: 'excel',
      styled: true,
      title: 'Branch List',
      headerColor: '7C3AED'
    });
  }

  exportToPdf() {
    const exportData = this.getExportData();
    this.exportService.export(exportData, {
      filename: 'Branches',
      format: 'pdf',
      title: 'Branch List',
      headerColor: '7C3AED'
    });
  }
}
