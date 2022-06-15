import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserDetail } from '../_models/userDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private _httpClient : HttpClient) { }

  getUser(userName : string | undefined){
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/' + userName);
  }

  updateUser(userDetail : UserDetail){
    return this._httpClient.put<UserDetail>(this.baseUrl + 'user', userDetail);
  }

  getUsers(){
    return this._httpClient.get<UserDetail[]>(this.baseUrl + 'user');
  }
}
