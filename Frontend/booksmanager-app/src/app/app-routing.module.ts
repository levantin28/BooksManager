import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainPageComponent } from './components/main-page/main-page.component';
import { CreateAuthorComponent } from './components/create-author/create-author.component';
import { CreateBookComponent } from './components/create-book/create-book.component';
import { UpdateAuthorComponent } from './components/update-author/update-author.component';
import { UpdateBookComponent } from './components/update-book/update-book.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { AuthorDetailsComponent } from './components/author-details/author-details.component';

const routes: Routes = [{
  path: '',
  redirectTo: '/home',
  pathMatch: 'full'
}, {
  path: 'home',
  component: MainPageComponent,
  data: { title: 'Home' }
},{
  path: 'create-author',
  component: CreateAuthorComponent,
  data: { title: 'Create Author' }
},{
  path: 'create-book',
  component: CreateBookComponent,
  data: { title: 'Create Book' }
},{
  path: 'update-author',
  component: UpdateAuthorComponent,
  data: { title: 'Update Author' }
},{
  path: 'update-book',
  component: UpdateBookComponent,
  data: { title: 'Update Book' }
},{
  path: 'book-details',
  component: BookDetailsComponent,
  data: { title: 'Book Details' }
},{
  path: 'author-details',
  component: AuthorDetailsComponent,
  data: { title: 'Author Details' }
},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
