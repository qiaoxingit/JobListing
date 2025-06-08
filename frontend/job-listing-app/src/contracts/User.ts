export interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    username: string;
    role: Role;
}

export type Role = 'Poster' | 'Viewer';