export interface User {
  id: number;
  username: string;
  email: string;
  fullName: string;
  role: string;
  department?: string;
  isActive: boolean;
  createdAt: string;
  lastLogin?: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
  user: User;
}

export interface RegisterRequest {
  username: string;
  email: string;
  fullName: string;
  role: string;
  department?: string;
  password: string;
}

