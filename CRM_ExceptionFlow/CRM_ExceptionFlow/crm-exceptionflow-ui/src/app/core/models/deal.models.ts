export interface Deal {
  id: number;
  title: string;
  description?: string;
  amount: number;
  stage: string;
  priority: string;
  customerId: number;
  assignedToUserId: number;
  assignedTo?: string;
  expectedCloseDate?: string;
  createdAt: string;
  updatedAt: string;
}

export interface DealRequest {
  title: string;
  description?: string;
  amount: number;
  stage: string;
  priority: string;
  customerId: number;
  assignedToUserId: number;
  expectedCloseDate?: string;
}

