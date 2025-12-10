import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DashboardService } from '../../core/services/dashboard.service';
import { ExceptionsService } from '../../core/services/exceptions.service';
import { DashboardSummary } from '../../core/models/dashboard.models';
import { ExceptionSummary } from '../../core/models/exception.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  readonly summary = signal<DashboardSummary | null>(null);
  readonly summaryLoading = signal(true);
  readonly latestExceptions = signal<ExceptionSummary[]>([]);
  readonly exceptionsLoading = signal(true);

  constructor(
    private readonly dashboardService: DashboardService,
    private readonly exceptionsService: ExceptionsService
  ) {}

  ngOnInit(): void {
    this.loadSummary();
    this.loadExceptions();
  }

  private loadSummary() {
    this.summaryLoading.set(true);
    this.dashboardService.getSummary().subscribe({
      next: (data) => this.summary.set(data),
      error: () => this.summary.set(null),
      complete: () => this.summaryLoading.set(false)
    });
  }

  private loadExceptions() {
    this.exceptionsLoading.set(true);
    this.exceptionsService.list({ pageSize: 5, pageNumber: 1, status: 'Open' }).subscribe({
      next: (result) => this.latestExceptions.set(result.items),
      error: () => this.latestExceptions.set([]),
      complete: () => this.exceptionsLoading.set(false)
    });
  }
}

