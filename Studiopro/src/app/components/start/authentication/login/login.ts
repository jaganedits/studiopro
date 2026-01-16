import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { InputText } from 'primeng/inputtext';
import { Password } from 'primeng/password';
import { Select } from 'primeng/select';
import { ThemeService } from '../../../../services/helpers/theme.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, InputText, Password, Select],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {

  constructor(
    public themeService: ThemeService
  ) { }

  toggleTheme() {
    this.themeService.toggleTheme();
  }
  onLogin() {
    this.themeService.toggleTheme();
  }
  username: any;
  password: any;
  branches: any[] | null | undefined;
  selectedBranch: any;
  errorMessage: any;

}
