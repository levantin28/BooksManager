import { Component, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Router } from '@angular/router';
import { Author } from '../../models/author';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateBookCommand } from '../../models/book';
import { EmitModification } from '../../services/emit-modification.service';

@Component({
  selector: 'app-create-book',
  templateUrl: './create-book.component.html',
  styleUrl: './create-book.component.css'
})
export class CreateBookComponent implements OnInit {
  authors:Author[]=[];
  
  createBookForm: FormGroup;

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
  /**
   *
   */
  constructor(private dataService:DataService, private router: Router, private formBuilder: FormBuilder, private emitModification: EmitModification) {
    this.createBookForm = this.formBuilder.group({
      Title: ['', Validators.required],
      Description: ['', Validators.required], 
      AuthorsIds: [[], Validators.required], 
      File: [null, Validators.required],
      CoverImagePath:['default']
    });
    
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if(this.createBookForm.valid){
      console.log(this.createBookForm.value);
      const formData = new FormData();
      formData.append('title', this.createBookForm.controls['Title'].value);
      formData.append('description', this.createBookForm.controls['Description'].value);
      formData.append('authorsIds', this.createBookForm.controls['AuthorsIds'].value);
      formData.append('file', this.createBookForm.controls['File'].value);
      formData.append('coverImagePath', this.createBookForm.controls['CoverImagePath'].value);
      
      this.dataService.postBook(formData).subscribe();
      this.backToMain();
    }
  }

  backToMain() {
    this.emitModification.emitModification();
    this.router.navigate(['/home']);
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.createBookForm.patchValue({
      File: file
    });
  }

}
