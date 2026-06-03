export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  isSuccess: boolean;
  message: string;
  result: string; // JWT token
}