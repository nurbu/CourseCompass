import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-logo',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="logo-container" [ngClass]="size">
      <div class="compass-circle">
        <!-- Cardinal Directions -->
        <div class="direction north">N</div>
        <div class="direction east">E</div>
        <div class="direction south">S</div>
        <div class="direction west">W</div>
        
        <!-- Bird SVG -->
        <svg class="bird-icon" viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg">
          <path d="M30 45 C35 35, 45 30, 55 35 C65 40, 75 45, 85 50 C80 55, 70 60, 60 58 C50 56, 45 58, 40 62 C35 58, 30 52, 30 45 Z" fill="#DC2626"/>
          <path d="M45 48 C50 42, 58 40, 65 43 C70 45, 72 48, 75 52 C70 54, 65 55, 60 54 C55 53, 50 52, 45 48 Z" fill="#DC2626"/>
          <path d="M35 50 C40 48, 45 50, 48 53 C45 55, 40 56, 35 54 Z" fill="#DC2626"/>
        </svg>
        
        <!-- Compass markers -->
        <div class="compass-marker top"></div>
        <div class="compass-marker right"></div>
        <div class="compass-marker bottom"></div>
        <div class="compass-marker left"></div>
      </div>
      
      <div class="logo-text" *ngIf="showText">
        <div class="brand-name">COURSE</div>
        <div class="brand-name">COMPASS</div>
      </div>
    </div>
  `,
  styles: [`
    .logo-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 12px;
      
      &.small {
        .compass-circle { width: 40px; height: 40px; }
        .direction { font-size: 10px; }
        .logo-text { display: none; }
      }
      
      &.medium {
        .compass-circle { width: 60px; height: 60px; }
        .direction { font-size: 12px; }
        .brand-name { font-size: 14px; }
      }
      
      &.large {
        .compass-circle { width: 120px; height: 120px; }
        .direction { font-size: 16px; font-weight: 600; }
        .brand-name { font-size: 24px; }
      }
    }

    .compass-circle {
      position: relative;
      width: 80px;
      height: 80px;
      border: 3px solid #374151;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      background: white;
      box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .direction {
      position: absolute;
      font-weight: 700;
      color: #374151;
      font-size: 14px;
      font-family: 'Arial', sans-serif;
      
      &.north { top: -8px; left: 50%; transform: translateX(-50%); }
      &.east { right: -8px; top: 50%; transform: translateY(-50%); }
      &.south { bottom: -8px; left: 50%; transform: translateX(-50%); }
      &.west { left: -8px; top: 50%; transform: translateY(-50%); }
    }

    .bird-icon {
      width: 50%;
      height: 50%;
      transform: rotate(-15deg);
    }

    .compass-marker {
      position: absolute;
      background: #374151;
      
      &.top, &.bottom { width: 2px; height: 8px; }
      &.left, &.right { width: 8px; height: 2px; }
      
      &.top { top: -1px; left: 50%; transform: translateX(-50%); }
      &.right { right: -1px; top: 50%; transform: translateY(-50%); }
      &.bottom { bottom: -1px; left: 50%; transform: translateX(-50%); }
      &.left { left: -1px; top: 50%; transform: translateY(-50%); }
    }

    .logo-text {
      text-align: center;
      line-height: 1.1;
    }

    .brand-name {
      font-weight: 900;
      color: #374151;
      letter-spacing: 1px;
      font-family: 'Arial Black', sans-serif;
      font-size: 18px;
    }
  `]
})
export class LogoComponent {
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() showText: boolean = true;
}
