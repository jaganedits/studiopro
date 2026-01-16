import { Injectable, signal, computed } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {

  // UX timing (tune as needed)
  private readonly _showDelay = 200;   // wait before showing spinner
  private readonly _minVisible = 400;  // keep visible at least this long

  // internal timers
  private showTimer: any = null;
  private hideTimer: any = null;
  private visibleSince = 0;

  // track concurrent requests
  private readonly _pendingRequests = signal(0);

  // visible loading state for UI
  private readonly _visible = signal(false);

  // public API (template binds to this)
  readonly isLoading = computed(() => this._visible());

  /**
   * Increment active request count
   */
  show(): void {
    this._pendingRequests.update(c => c + 1);

    // already visible → nothing to do
    if (this._visible()) return;

    // schedule delayed show (avoid flicker on fast requests)
    if (!this.showTimer) {
      this.showTimer = setTimeout(() => {
        this.showTimer = null;

        // only show if there are still pending requests
        if (this._pendingRequests() > 0) {
          this.visibleSince = performance.now();
          this._visible.set(true);
        }
      }, this._showDelay);
    }
  }

  /**
   * Decrement active request count
   */
  hide(): void {
    this._pendingRequests.update(c => Math.max(0, c - 1));

    // if all requests finished before showing → cancel show timer
    if (this.showTimer && this._pendingRequests() === 0) {
      clearTimeout(this.showTimer);
      this.showTimer = null;
      return;
    }

    // not visible yet → nothing more to do
    if (!this._visible()) return;

    const elapsed = performance.now() - this.visibleSince;

    // enforce minimum visible duration
    if (elapsed < this._minVisible) {
      if (this.hideTimer) clearTimeout(this.hideTimer);

      this.hideTimer = setTimeout(() => {
        this.hideTimer = null;
        if (this._pendingRequests() === 0) {
          this._visible.set(false);
        }
      }, this._minVisible - elapsed);

      return;
    }

    // safe to hide immediately
    if (this._pendingRequests() === 0) {
      this._visible.set(false);
    }
  }

  /**
   * Force hide everything (navigation reset, etc.)
   */
  forceHide(): void {
    if (this.showTimer) clearTimeout(this.showTimer);
    if (this.hideTimer) clearTimeout(this.hideTimer);

    this.showTimer = null;
    this.hideTimer = null;

    this._pendingRequests.set(0);
    this._visible.set(false);
  }

  /**
   * Optional: backward-compatible accessor
   */
  get loading(): boolean {
    return this.isLoading();
  }

  /**
   * Debug / metrics
   */
  get activeRequestCount(): number {
    return this._pendingRequests();
  }
}
