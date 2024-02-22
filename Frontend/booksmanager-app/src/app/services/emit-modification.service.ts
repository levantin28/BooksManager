import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmitModification {
  private emitModifications = new Subject<void>();
  modification$ = this.emitModifications.asObservable();

  constructor() {}

  emitModification() {
    this.emitModifications.next();
  }
}