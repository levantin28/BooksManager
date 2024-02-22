import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../services/data.service';
import { Book } from '../../models/book';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent {
  dataReceived: any;
  book:Book;
constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router,) {
  this.route.paramMap.subscribe(params => {
    this.dataReceived = JSON.parse(params.get('id')!);
  });

  this.book = new Book();
  
}

ngOnInit(): void {

  this.dataService.getBookById(this.dataReceived).subscribe({
    next: (res) => {
      res.queryResults.forEach((element: any) => {
        let book = new Book();
        this.book.id = element.id;
        this.book.title = element.title;
        this.book.authors = element.authors;
        this.book.coverImage = element.coverImage;
        this.book.description = element.description;
      });

    },
    error: (err) => { console.log(err.message); }
  })
}
backToMain() {
  this.router.navigate(['/home']);
}
}
