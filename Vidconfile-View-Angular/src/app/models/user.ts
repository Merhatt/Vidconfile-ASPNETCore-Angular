import { Video } from './video';

export interface User {
    id: string;
    username: string;
    created: Date;
    profilePhotoUrl: string;
    subscriberCount: number;
    videos?: Video[];
}
