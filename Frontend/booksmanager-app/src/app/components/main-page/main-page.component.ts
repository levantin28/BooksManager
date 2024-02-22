import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Book } from '../../models/book';
import { Author } from '../../models/author';
import { MatTableDataSource } from '@angular/material/table';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { EmitModification } from '../../services/emit-modification.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent implements OnInit {

  constructor(private dataService: DataService, private router: Router, private emitModification: EmitModification) {
    
  }

  booksDataSource!: MatTableDataSource<any>;
  authorsDataSource!: MatTableDataSource<any>;
  books:Book[]=[];
  authors:Author[]=[];
  displayedColumnsBooks: string[] = ["id", "title", "description", "actions"];
  displayedColumnsAuthors: string[] = ["id", "name", "actions"];

  ngOnInit() {
    this.refreshTables();
    this.emitModification.modification$.subscribe(() => {
      this.refreshTables();
    });
  }

  refreshTables(){
    this.dataService.getAllBooks().subscribe({
      next: (res) => {
        res.queryResults[0].forEach((element: any) => {
          let book = new Book();
          book.id = element.id;
          book.title = element.title;
          book.description = element.description;
          book.authors = element.authors;

          this.books.push(book);
        });
        this.booksDataSource = new MatTableDataSource(this.books);

      },
      error: (err) => { console.log(err.message); }
    })

    this.dataService.getAllAuthors().subscribe({
      next: (res) => {
        res.queryResults[0].forEach((element: any) => {
          let author = new Author();
          author.id = element.id;
          author.name = element.name;
          author.books = element.books;

          this.authors.push(author);
        });
        this.authorsDataSource = new MatTableDataSource(this.authors);
      },
      error: (err) => { console.log(err.message); }
    })
  }

  showDetailsBook(event: any, row: Book) {
    this.router.navigate(['/book-details', { id: JSON.stringify(row.id) }]);
  }

  showUpdateBook(event: any, row: Book) {
    this.router.navigate(['/update-book', { data: JSON.stringify(row) }]);
  }

  deleteBook(event: any, row: Book) {
    this.dataService.deleteBook(row.id.toString()).subscribe();
  }

  showDetailsAuthor(event: any, row: Author) {
    this.router.navigate(['/author-details', { id: JSON.stringify(row.id) }]);
  }

  showUpdateAuthor(event: any, row: Author) {
    this.router.navigate(['/update-author', { data: JSON.stringify(row) }]);
  }

  deleteAuthor(event: any, row: Author) {
    this.dataService.deleteAuthor(row.id.toString()).subscribe();
  }

  navigateToCreateBook(event:any){
    this.router.navigate(["/create-book"]);
  }

  navigateToCreateAuthor(event:any){
    this.router.navigate(["/create-author"]);
  }
}
