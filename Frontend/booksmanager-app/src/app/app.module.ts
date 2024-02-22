import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatIconModule} from '@angular/material/icon';
import { DataService } from './services/data.service';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateAuthorComponent } from './components/create-author/create-author.component';
import { CreateBookComponent } from './components/create-book/create-book.component';
import { UpdateBookComponent } from './components/update-book/update-book.component';
import { UpdateAuthorComponent } from './components/update-author/update-author.component';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { AuthorDetailsComponent } from './components/author-details/author-details.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    CreateAuthorComponent,
    CreateBookComponent,
    UpdateBookComponent,
    UpdateAuthorComponent,
    BookDetailsComponent,
    AuthorDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatSelectModule,
    MatIconModule,
    MatInputModule
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ],
  providers: [DataService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule { }