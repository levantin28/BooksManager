import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../models/book';
import { UpdateAuthorCommand } from '../../models/author';
import { EmitModification } from '../../services/emit-modification.service';

@Component({
  selector: 'app-update-author',
  templateUrl: './update-author.component.html',
  styleUrl: './update-author.component.css'
})
export class UpdateAuthorComponent {
  dataReceived: any;
  updateAuthorForm: FormGroup;
  books:Book[]=[];
  constructor(private dataService: DataService, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder, private emitModification: EmitModification) {
    this.route.paramMap.subscribe(params => {
      this.dataReceived = JSON.parse(params.get('data')!);
    });

    this.updateAuthorForm = this.formBuilder.group({
      Id: [this.dataReceived.id, Validators.required,],
      Name: [this.dataReceived.name, Validators.required],
      BooksIds: [[], Validators.required],
    });

    this.updateAuthorForm.controls['Id'].disable();
  }

  ngOnInit(): void {

    this.dataService.getAllBooks().subscribe({
      next: (res) => {
        res.queryResults[0].forEach((element: any) => {
          let book = new Book();
          book.id = element.id;
          book.title = element.title;
          book.authors = element.authors;

          this.books.push(book);
        });

      },
      error: (err) => { console.log(err.message); }
    })
  }

  backToMain() {
    this.emitModification.emitModification();
    this.router.navigate(['/home']);
  }

  onSubmit(event: any) {
    event.preventDefault();

    if (this.updateAuthorForm.valid) {
      let updateAuthorCommand = new UpdateAuthorCommand();
      updateAuthorCommand.id = this.dataReceived.id;
      updateAuthorCommand.name = this.updateAuthorForm.controls['Name'].value;
      updateAuthorCommand.booksIds.push(this.updateAuthorForm.controls['BooksIds'].value);

      this.dataService.putAuthor(updateAuthorCommand).subscribe();

      this.backToMain();
    }
  }
}
