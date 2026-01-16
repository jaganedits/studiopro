import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly STORAGE_KEY = 'studio_theme';
  private isDark = signal(true);

  readonly darkMode = this.isDark.asReadonly();

  initTheme() {
    const stored = localStorage.getItem(this.STORAGE_KEY);
    const isDarkMode = stored ? stored === 'dark' : true;
    this.isDark.set(isDarkMode);
    this.applyTheme(isDarkMode);
  }

  toggleTheme() {
    const newMode = !this.isDark();
    this.isDark.set(newMode);
    localStorage.setItem(this.STORAGE_KEY, newMode ? 'dark' : 'light');
    this.applyTheme(newMode);
  }

  private applyTheme(isDark: boolean) {
    if (isDark) {
      document.body.classList.remove('light-mode');
      document.body.classList.add('dark-mode');
    } else {
      document.body.classList.remove('dark-mode');
      document.body.classList.add('light-mode');
    }
  }
}
