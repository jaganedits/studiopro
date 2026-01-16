import { Component, inject } from '@angular/core';
import { LoadingService } from '../../../services/helpers';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  imports: [CommonModule],
  templateUrl: './loading-spinner.html',
  styleUrl: './loading-spinner.scss',
})
export class LoadingSpinner {
  loadingService = inject(LoadingService);

}
