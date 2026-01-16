import { Component, OnInit, AfterViewChecked, ElementRef } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuItem } from 'primeng/api';
import { PanelMenuModule } from 'primeng/panelmenu';
import { AvatarModule } from 'primeng/avatar';
import { TooltipModule } from 'primeng/tooltip';
import { ThemeService } from '../../../../services/helpers/theme.service';

@Component({
  selector: 'app-left-menu',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    PanelMenuModule,
    AvatarModule,
    TooltipModule,
  ],
  templateUrl: './left-menu.html',
  styleUrl: './left-menu.scss',
})
export class LeftMenu implements OnInit, AfterViewChecked {
  userName = 'Admin';
  userRole = 'Administrator';
  userInitial = 'A';
  isCollapsed = false;
  private tooltipsApplied = false;

  menuItems: MenuItem[] = [];

  constructor(
    public themeService: ThemeService,
    private router: Router,
    private elementRef: ElementRef
  ) {}

  ngOnInit() {
    this.menuItems = [
      {
        label: 'Main',
        expanded: true,
        items: [
          {
            label: 'Dashboard',
            icon: 'pi pi-th-large',
            routerLink: ['/home/dashboard'],
          },
        ],
      },
      {
        label: 'Billing',
        expanded: true,
        items: [
          {
            label: 'New Bill',
            icon: 'pi pi-plus',
            routerLink: ['/home/new-bill'],
          },
          {
            label: 'Event Booking',
            icon: 'pi pi-calendar',
            routerLink: ['/home/event-booking'],
          },
          {
            label: 'Quotation',
            icon: 'pi pi-file',
            routerLink: ['/home/quotation'],
          },
          {
            label: 'Invoices',
            icon: 'pi pi-file-edit',
            routerLink: ['/home/invoices'],
          },
        ],
      },
      {
        label: 'Masters',
        expanded: true,
        items: [
          {
            label: 'Categories',
            icon: 'pi pi-folder',
            routerLink: ['/home/categories'],
          },
          {
            label: 'Frames',
            icon: 'pi pi-stop',
            routerLink: ['/home/frames'],
          },
          {
            label: 'Photos',
            icon: 'pi pi-image',
            routerLink: ['/home/photos'],
          },
          {
            label: 'Services',
            icon: 'pi pi-cog',
            routerLink: ['/home/services'],
          },
          {
            label: 'Packages',
            icon: 'pi pi-box',
            routerLink: ['/home/packages'],
          },
        ],
      },
      {
        label: 'Records',
        expanded: true,
        items: [
          {
            label: 'Customers',
            icon: 'pi pi-user',
            routerLink: ['/home/customers'],
          },
          {
            label: 'Expenses',
            icon: 'pi pi-dollar',
            routerLink: ['/home/expenses'],
          },
        ],
      },
      {
        label: 'Admin',
        expanded: true,
        items: [
          {
            label: 'Company',
            icon: 'pi pi-building',
            routerLink: ['/home/company'],
          },
          {
            label: 'Branches',
            icon: 'pi pi-map-marker',
            routerLink: ['/home/branches'],
          },
          {
            label: 'Users',
            icon: 'pi pi-users',
            routerLink: ['/home/users'],
          },
          {
            label: 'Roles',
            icon: 'pi pi-shield',
            routerLink: ['/home/roles'],
          },
          {
            label: 'Role Mapping',
            icon: 'pi pi-sitemap',
            routerLink: ['/home/cRolewiseScreenMapping'],
          },
        ],
      },
    ];
  }

  ngAfterViewChecked() {
    if (this.isCollapsed && !this.tooltipsApplied) {
      this.applyTooltips();
      this.tooltipsApplied = true;
    } else if (!this.isCollapsed && this.tooltipsApplied) {
      this.removeTooltips();
      this.tooltipsApplied = false;
    }
  }

  private applyTooltips() {
    setTimeout(() => {
      const menuItems = this.elementRef.nativeElement.querySelectorAll('.p-panelmenu-item-content');
      menuItems.forEach((item: HTMLElement) => {
        const label = item.querySelector('.p-panelmenu-item-label')?.textContent;
        if (label) {
          item.setAttribute('data-tooltip', label);
        }
      });
    }, 100);
  }

  private removeTooltips() {
    const menuItems = this.elementRef.nativeElement.querySelectorAll('.p-panelmenu-item-content');
    menuItems.forEach((item: HTMLElement) => {
      item.removeAttribute('data-tooltip');
    });
  }

  toggleTheme() {
    this.themeService.toggleTheme();
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.tooltipsApplied = false; // Reset to reapply tooltips
  }

  logout() {
    this.router.navigate(['/login']);
  }
}
