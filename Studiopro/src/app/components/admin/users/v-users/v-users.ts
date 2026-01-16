import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TagModule } from 'primeng/tag';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-v-users',
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
    PasswordModule
  ],
  templateUrl: './v-users.html',
  styleUrl: './v-users.scss',
  providers: [MessageService, ConfirmationService],

})
export class VUsers {


  constructor(
    private fb: FormBuilder,
    private router: Router,
    private messageService: MessageService
  ) { }
  openAddDialog() {
    this.router.navigate(['home/c-users']);
  }
  users(): any[] {
    throw new Error('Method not implemented.');
  }
  getRoleSeverity(arg0: any): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | null | undefined {
    throw new Error('Method not implemented.');
  }
  editUser(_t23: any) {
    throw new Error('Method not implemented.');
  }
  toggleStatus(_t23: any) {
    throw new Error('Method not implemented.');
  }
  deleteUser(_t23: any) {
    throw new Error('Method not implemented.');
  }

}
