import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { LoadingSpinner } from "./shared/components/loading-spinner/loading-spinner";
import { OfflineBanner } from "./shared/components/offline-banner/offline-banner";
import { LoadingService } from './services/helpers';
import { Subscription } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop'; // Add this import
import { CommonModule } from '@angular/common';
import { Toast } from "primeng/toast";
import { ConfirmDialog } from "primeng/confirmdialog";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoadingSpinner, OfflineBanner, CommonModule, Toast, ConfirmDialog],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected title = 'Studiopro';
  loading: boolean = false;
  private loadingSub!: Subscription;

  private router = inject(Router);
  private loadingService = inject(LoadingService);
  protected isLoading = this.loadingService.isLoading;
  ngOnInit(): void {
   
  }

  
}