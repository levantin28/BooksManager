
import { BookAuthor } from "./book-author";

export class Book {
    id!: number;
    title!: string;
    description!: string;
    authors!: BookAuthor[];
    coverImage!: string;
  }

  export class CreateBookCommand{
    title!: string;
    description!:string;
    coverImagePath!:string;
    file!:any;
    authorsIds:number[]=[];
  }

  export class UpdateBookCommand{
    id!:number;
    title!: string;
    description!:string;
    authorsIds:number[]=[];
  }