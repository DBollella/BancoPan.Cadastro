import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [MenubarModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent {
  items: MenuItem[] = [];

  constructor(private router: Router) {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-home',
        command: () => this.router.navigate(['/'])
      },
      {
        label: 'Pessoas Físicas',
        icon: 'pi pi-user',
        command: () => this.router.navigate(['/pessoas-fisicas'])
      },
      {
        label: 'Pessoas Jurídicas',
        icon: 'pi pi-briefcase',
        command: () => this.router.navigate(['/pessoas-juridicas'])
      }
    ];
  }
}
