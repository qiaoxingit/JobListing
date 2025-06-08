export interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    username: string;
    role: Role;
}

export const Role = {
    Viewer: 0,
    Poster: 1,
} as const;

export type Role = (typeof Role)[keyof typeof Role];

