export interface AuthenticationResponse {
    isAuthenticated: boolean;
    userId: string;
    userName: string;
    firstName: string;
    lastName: string;
    role: string;
}