import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { Router } from '@angular/router';
import { EmitModification } from '../../services/emit-modification.service';

@Component({
  selector: 'app-create-author',
  templateUrl: './create-author.component.html',
  styleUrl: './create-author.component.css'
})
export class CreateAuthorComponent {
  createAuthorForm:FormGroup;

  /**
   *
   */
  constructor(private dataService:DataService, private router: Router, private formBuilder: FormBuilder, private emitModification: EmitModification) {
    this.createAuthorForm = this.formBuilder.group({
      Name: ['', Validators.required],
    });
    
  }
  
  onSubmit(event: Event) {
    event.preventDefault();
    if(this.createAuthorForm.valid){

      this.dataService.postAuthor(this.createAuthorForm.value).subscribe();
      this.backToMain();
      
    }
  }

  backToMain() {
    this.emitModification.emitModification();
    this.router.navigate(['/home']);
    
  }
}
