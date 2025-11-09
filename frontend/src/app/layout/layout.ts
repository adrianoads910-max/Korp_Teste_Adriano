import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

// 📌 ÍCONES HEROICONS
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import {
  heroCube,
  heroClipboardDocument,
  heroMoon,
  heroSun
} from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-layout',
  standalone: true,
  templateUrl: './layout.html',
  styleUrls: ['./layout.scss'],
  imports: [
    RouterOutlet,
    RouterLink,
    NgIconComponent,
    CommonModule
  ],
  providers: [
    provideIcons({
      heroCube,
      heroClipboardDocument,
      heroMoon,
      heroSun
    })
  ]
})
export class LayoutComponent {
  dark = false;

  toggleDarkMode() {
    this.dark = !this.dark;
    document.documentElement.classList.toggle('dark', this.dark);
  }
}
