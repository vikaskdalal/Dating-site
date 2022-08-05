import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  private _currentUserSource = new BehaviorSubject<User | null>(null);

  currentUser$ = this._currentUserSource.asObservable();

  constructor(private _http: HttpClient, private _presenceService: PresenceService) { }

  login(model: any) {
    return this._http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(response => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this._currentUserSource.next(user);
          this._presenceService.createHubConnection(user);
        }
      })
    )
  }

  register(model: any) {
    return this._http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(response => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this._currentUserSource.next(user);
          this._presenceService.createHubConnection(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User | null) {
    this._currentUserSource.next(user);
  }

  isTokenExpired(user: User): boolean {
    const expires = new Date(user.tokenExpire);
    const timeout = expires.getTime() - Date.now();
    return timeout < 0;
  }

  logout() {
    localStorage.removeItem("user");
    this._currentUserSource.next(null);
    this._presenceService.stopHubConnection();
  }
}
