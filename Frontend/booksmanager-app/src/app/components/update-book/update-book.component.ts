import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../services/data.service';
import { Author } from '../../models/author';
import { UpdateBookCommand } from '../../models/book';
import { EmitModification } from '../../services/emit-modification.service';

@Component({
  selector: 'app-update-book',
  templateUrl: './update-book.component.html',
  styleUrl: './update-book.component.css'
})
export class UpdateBookComponent implements OnInit {
  dataReceived: any;
  updateBookForm:FormGroup;
  authors:Author[]=[];
  constructor(private dataService:DataService, private route: ActivatedRoute, private router:Router, private formBuilder:FormBuilder, private emitModification: EmitModification) {
    this.route.paramMap.subscribe(params => {
      this.dataReceived = JSON.parse(params.get('data')!);
    });

    this.updateBookForm = this.formBuilder.group({
      Id: [this.dataReceived.id, Validators.required, ],
      Title: [this.dataReceived.title, Validators.required],
      Description: [this.dataReceived.description, Validators.required], 
      AuthorsIds: [[], Validators.required], 
    });

    this.updateBookForm.controls['Id'].disable();
    this.updateBookForm.controls['Title'].disable();

  }

  ngOnInit(): void {
    this.dataService.getAllAuthors().subscribe({
      next: (res) => {
        res.queryResults[0].forEach((element: any) => {
          let author = new Author();
          author.id = element.id;
          author.name = element.name;
          author.books = element.books;

          this.authors.push(author);
        });

      },
      error: (err) => { console.log(err.message); }
    })
  }

  backToMain() {
    this.emitModification.emitModification();
    this.router.navigate(['/home']);
  }

  onSubmit(event:any){
    event.preventDefault();
    if(this.updateBookForm.valid){
      let updateBookCommand = new UpdateBookCommand();
      updateBookCommand.id = this.dataReceived.id;
      updateBookCommand.title = this.dataReceived.title;
      updateBookCommand.description = this.updateBookForm.controls['Description'].value;
      updateBookCommand.authorsIds.push(this.updateBookForm.controls['AuthorsIds'].value);

      this.dataService.putBook(updateBookCommand).subscribe();

      this.backToMain();
    }
  }
}
