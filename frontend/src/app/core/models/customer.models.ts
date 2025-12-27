import { Deal } from './deal.models';
import { Interaction } from './interaction.models';

export interface CustomerSummary {
  id: number;
  name: string;
  email?: string;
  phone?: string;
  company?: string;
  status: string;
  assignedTo?: string;
  assignedToUserId: number;
  createdAt: string;
  updatedAt: string;
}

export interface CustomerDetail extends CustomerSummary {
  address?: string;
  deals: Deal[];
  interactions: Interaction[];
}

export interface CustomerRequest {
  name: string;
  email?: string;
  phone?: string;
  company?: string;
  address?: string;
  status: string;
  assignedToUserId: number;
}

