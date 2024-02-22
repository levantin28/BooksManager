
import { BookAuthor } from "./book-author";

export class Author {
    id!: number;
    name!: string;
    books!: BookAuthor[];
  }

  export class UpdateAuthorCommand {
    id!: number;
    name!: string;
    booksIds: number[] = [];
  }