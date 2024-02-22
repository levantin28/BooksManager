import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { Book, CreateBookCommand, UpdateBookCommand } from '../models/book';
import { Author, UpdateAuthorCommand } from '../models/author';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(private http: HttpClient, private configService: ConfigService) {}

  postBook(data: FormData): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Books/create`;
    return this.http.post(apiUrl, data);
  }

  putBook(data: UpdateBookCommand): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Books/update`;
    return this.http.put(apiUrl, data);
  }

  deleteBook(id: string): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Books/delete?id=${id}`;
    return this.http.delete(apiUrl);
  }

  getBookById(id: string): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Books/${id}`;
    return this.http.get<Book>(apiUrl);
  }

  getAllBooks(): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Books/all`;
    return this.http.get<any>(apiUrl);
  }

  postAuthor(data: Author): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Authors/create`;
    return this.http.post(apiUrl, data);
  }

  putAuthor(data: UpdateAuthorCommand): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Authors/update`;
    return this.http.put(apiUrl, data);
  }

  deleteAuthor(id: string): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Authors/delete?id=${id}`;
    return this.http.delete(apiUrl);
  }

  getAuthorById(id: string): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Authors/${id}`;
    return this.http.get<Author>(apiUrl);
  }

  getAllAuthors(): Observable<any> {
    const apiUrl = `${this.configService.apiUrl}/api/Authors/all`;
    return this.http.get<any>(apiUrl);
  }
}
