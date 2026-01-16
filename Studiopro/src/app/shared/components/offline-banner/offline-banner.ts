import { Component, inject, computed, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NetworkService, NetworkStatus } from '../../../services/helpers/network.service';

type BannerState = 'hidden' | 'offline' | 'server-unreachable' | 'back-online';

@Component({
  selector: 'app-offline-banner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offline-banner.html',
  styleUrl: './offline-banner.scss',
})
export class OfflineBanner {
  private networkService = inject(NetworkService);
  private previousStatus = signal<NetworkStatus>(NetworkStatus.Online);
  private showBackOnline = signal(false);
  private hideTimeout: ReturnType<typeof setTimeout> | null = null;

  // Stranger Things flickering lights
  readonly lights = Array.from({ length: 26 }, (_, i) => ({
    id: i,
    delay: `${Math.random() * 2}s`
  }));

  readonly bannerState = computed<BannerState>(() => {
    const status = this.networkService.networkStatus();

    if (this.showBackOnline()) {
      return 'back-online';
    }

    if (status === NetworkStatus.Offline) {
      return 'offline';
    }

    if (status === NetworkStatus.ServerUnreachable) {
      return 'server-unreachable';
    }

    return 'hidden';
  });

  readonly isVisible = computed(() => this.bannerState() !== 'hidden');

  constructor() {
    // Track status changes to show "back online" message
    effect(() => {
      const currentStatus = this.networkService.networkStatus();
      const prevStatus = this.previousStatus();

      // If transitioning from offline/unreachable to online
      if (
        (prevStatus === NetworkStatus.Offline || prevStatus === NetworkStatus.ServerUnreachable) &&
        currentStatus === NetworkStatus.Online
      ) {
        this.showBackOnlineBanner();
      }

      this.previousStatus.set(currentStatus);
    });
  }

  private showBackOnlineBanner(): void {
    // Clear any existing timeout
    if (this.hideTimeout) {
      clearTimeout(this.hideTimeout);
    }

    this.showBackOnline.set(true);

    // Auto-hide after 3 seconds
    this.hideTimeout = setTimeout(() => {
      this.showBackOnline.set(false);
    }, 3000);
  }

  dismissBackOnline(): void {
    if (this.hideTimeout) {
      clearTimeout(this.hideTimeout);
    }
    this.showBackOnline.set(false);
  }

  retry(): void {
    window.location.reload();
  }
}
