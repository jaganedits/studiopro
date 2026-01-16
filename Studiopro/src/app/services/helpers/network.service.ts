import { Injectable, signal, computed, OnDestroy } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

/**
 * Network status enum
 */
export enum NetworkStatus {
  Online = 'online',
  Offline = 'offline',
  ServerUnreachable = 'server-unreachable',
}

/**
 * Modern Network Service
 * - Uses Angular signals for reactive state
 * - Monitors browser online/offline status
 * - Tracks server reachability
 */
@Injectable({
  providedIn: 'root',
})
export class NetworkService implements OnDestroy {
  // Private signals
  private readonly _isOnline = signal<boolean>(navigator.onLine);
  private readonly _isServerReachable = signal<boolean>(true);

  // Public computed signals
  readonly isOnline = computed(() => this._isOnline());
  readonly isServerReachable = computed(() => this._isServerReachable());

  readonly networkStatus = computed<NetworkStatus>(() => {
    if (!this._isOnline()) {
      return NetworkStatus.Offline;
    }
    if (!this._isServerReachable()) {
      return NetworkStatus.ServerUnreachable;
    }
    return NetworkStatus.Online;
  });

  readonly isFullyConnected = computed(() => this._isOnline() && this._isServerReachable());

  // Observables for components using RxJS
  readonly isOnline$: Observable<boolean> = toObservable(this.isOnline);
  readonly isServerReachable$: Observable<boolean> = toObservable(this.isServerReachable);
  readonly networkStatus$: Observable<NetworkStatus> = toObservable(this.networkStatus);

  // Event listener references for cleanup
  private onlineHandler = () => this.handleOnline();
  private offlineHandler = () => this.handleOffline();

  constructor() {
    this.initNetworkListeners();
  }

  ngOnDestroy(): void {
    window.removeEventListener('online', this.onlineHandler);
    window.removeEventListener('offline', this.offlineHandler);
  }

  /**
   * Initialize browser network event listeners
   */
  private initNetworkListeners(): void {
    window.addEventListener('online', this.onlineHandler);
    window.addEventListener('offline', this.offlineHandler);
  }

  private handleOnline(): void {
    this._isOnline.set(true);
    // Reset server status when coming back online
    this._isServerReachable.set(true);
  }

  private handleOffline(): void {
    this._isOnline.set(false);
  }

  /**
   * Called by interceptor when API request fails with status 0
   */
  setServerUnreachable(): void {
    this._isServerReachable.set(false);
  }

  /**
   * Called by interceptor when API request succeeds
   */
  setServerReachable(): void {
    this._isServerReachable.set(true);
  }

  /**
   * Get user-friendly network status message
   */
  getStatusMessage(): string {
    switch (this.networkStatus()) {
      case NetworkStatus.Offline:
        return 'No internet connection. Please check your network.';
      case NetworkStatus.ServerUnreachable:
        return 'Unable to reach the server. Please try again later.';
      case NetworkStatus.Online:
        return 'Connected';
    }
  }
}