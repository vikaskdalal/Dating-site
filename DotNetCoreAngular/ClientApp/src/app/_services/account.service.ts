import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  private currentUserSource = new BehaviorSubject<User | null>(null);
  
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private _http : HttpClient) { }

  login(model : any){
    return this._http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(response => {
          const user = response;
          if(user){
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
      })
    )
  }

  register(model : any){
    return this._http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(response => {
          const user = response;
          if(user){
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
          return user;
      })
    )
  }

  setCurrentUser(user : User | null){
    this.currentUserSource.next(user);
  }

  isTokenExpired(user : User) : boolean {
      const expires = new Date(user.tokenExpire);
      const timeout = expires.getTime() - Date.now();
      return timeout < 0;
  }

  logout(){
    localStorage.removeItem("user");
    this.currentUserSource.next(null);
  }
}
