import { Video } from './video';

export interface User {
    id: string;
    username: string;
    created: Date;
    photoUrl: string;
    videos?: Video[];
}
