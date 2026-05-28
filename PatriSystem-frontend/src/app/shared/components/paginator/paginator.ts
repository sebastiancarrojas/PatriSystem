import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-paginator',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './paginator.html',
  styleUrl: './paginator.scss'
})
export class PaginatorComponent implements OnChanges {
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;
  @Input() visiblePages: number = 5;

  @Output() pageChange = new EventEmitter<number>();

  pages: number[] = [];

  ngOnChanges(): void {
    this.calculatePages();
  }

  calculatePages(): void {
    const half = Math.floor(this.visiblePages / 2);
    let start = this.currentPage - half;
    let end = this.currentPage + half;

    if (start <= 0) {
      start = 1;
      end = Math.min(this.visiblePages, this.totalPages);
    }

    if (end > this.totalPages) {
      end = this.totalPages;
      start = Math.max(1, this.totalPages - this.visiblePages + 1);
    }

    this.pages = Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) return;
    this.pageChange.emit(page);
  }
}