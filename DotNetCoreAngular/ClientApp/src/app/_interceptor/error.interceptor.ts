import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private _toastr : ToastrService, private _router : Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if(!error){
            return throwError(() => error);
          }

          switch(error.status){
            case 400:
              if(error.error.errors){
                throw this.extractModelErrors(error.error.errors);
              }
              else
                this._toastr.error(error.statusText, error.status.toString());
              break;
            case 401:
              this.handle401(error);
              break;
            case 404:
              this._router.navigateByUrl('/not-found');
              break;
            case 500:
              this.handle500(error);
              break;
            default:
              this._toastr.error('Something went wrong');
              break;
            }
          
          return throwError(() => error);
        })
      )
  }

  extractModelErrors(errors : string[]) : string[]{
    const modelErrors = [];
    for(const key in errors)
    {
      if(errors[key])
        modelErrors.push(errors[key]);
    }
    return modelErrors.flat();
  }

  handle401(error : HttpErrorResponse){
    this._toastr.error(error.error, error.status.toString());
  }

  handle500(error : HttpErrorResponse){
    const navigateExtra : NavigationExtras = {state : {error : error.error}};
    this._router.navigateByUrl('/server-error', navigateExtra);
  }
}
