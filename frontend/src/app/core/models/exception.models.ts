export interface ExceptionSummary {
  id: number;
  projectId: string;
  module: string;
  title: string;
  status: string;
  priority: string;
  reportedByUserId: number;
  assignedToUserId?: number;
  reportedBy?: string;
  assignedTo?: string;
  reportedAt: string;
  resolvedAt?: string;
}

export interface ExceptionDetail extends ExceptionSummary {
  description: string;
  stackTrace?: string;
  resolutionNotes?: string;
  history: ExceptionHistory[];
  recommendations: Recommendation[];
}

export interface ExceptionHistory {
  id: number;
  status: string;
  changedByUserName: string;
  notes?: string;
  changedAt: string;
}

export interface Recommendation {
  id: number;
  recommendationText: string;
  confidenceScore: number;
  model: string;
  source: string;
  isFromDatabase: boolean;
  generatedAt: string;
  wasHelpful?: boolean;
}

export interface ExceptionRequest {
  projectId: string;
  module: string;
  title: string;
  description: string;
  stackTrace?: string;
  status: string;
  priority: string;
  reportedByUserId: number;
  assignedToUserId?: number;
  resolutionNotes?: string;
}

