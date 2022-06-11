import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private _accountService : AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser : User | null | undefined;

    this._accountService.currentUser$.subscribe(user => currentUser = user);

    if(currentUser){
      request = request.clone({
        setHeaders: {
          'Authorization': `Bearer ${currentUser.token}`
        }
      })

    }
    return next.handle(request);
  }
}
