import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private _httpClient : HttpClient) { }

  getUser(userName : string | null){
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/' + userName);
  }
}
