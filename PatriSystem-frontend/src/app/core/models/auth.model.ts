export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  isSuccess: boolean;
  message: string;
  result: string; // JWT token
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  email: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ApiResponse<T = unknown> {
  isSuccess: boolean;
  message: string;
  errors: string[];
  result: T | null;
}