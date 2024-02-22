import { Component } from '@angular/core';
import { Author } from '../../models/author';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../models/book';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-author-details',
  templateUrl: './author-details.component.html',
  styleUrl: './author-details.component.css'
})
export class AuthorDetailsComponent {
  dataReceived: any;
  author:Author;
constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router,) {
  this.route.paramMap.subscribe(params => {
    this.dataReceived = JSON.parse(params.get('id')!);
  });

  this.author = new Author();
  
}

ngOnInit(): void {

  this.dataService.getAuthorById(this.dataReceived).subscribe({
    next: (res) => {
      res.queryResults.forEach((element: any) => {
        this.author.id = element.id;
        this.author.name = element.name;
        this.author.books = element.books;
      });

    },
    error: (err) => { console.log(err.message); }
  })
}
backToMain() {
  this.router.navigate(['/home']);
}
}
