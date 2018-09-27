export class VideoComment {
    constructor() {}

    public id: string;
    public videoId: string;
    public authorId: string;
    public created: Date;
    public commentText: string;
    public authorUsername: string;
    public authorProfilePhotoUrl: string;
}
