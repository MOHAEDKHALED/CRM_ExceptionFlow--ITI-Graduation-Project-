export interface Interaction {
  id: number;
  type: string;
  subject: string;
  notes?: string;
  interactionDate: string;
  customerId: number;
  userId: number;
  userFullName?: string;
  createdAt: string;
}

export interface InteractionRequest {
  type: string;
  subject: string;
  notes?: string;
  interactionDate: string;
  customerId: number;
  userId: number;
}

