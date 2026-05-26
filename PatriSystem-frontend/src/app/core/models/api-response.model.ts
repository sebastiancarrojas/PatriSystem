export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  result: T;
  errors: string[];
  exception: string | null;
}