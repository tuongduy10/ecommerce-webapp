import { IComment } from "src/_cores/_interfaces/product.interface";

export interface IUserComment {
    id: string;
    data?: IComment;
    type: 'post' | 'comment' | 'reply'
}