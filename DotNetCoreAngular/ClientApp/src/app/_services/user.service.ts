import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Photo } from '../_models/photo';
import { UserDetail } from '../_models/userDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  baseUrl = environment.apiUrl;

  constructor(private _httpClient : HttpClient) { }

  getUser(email : string | undefined){
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/' + email);
  }

  getByUsername(userName : string | null){
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/getbyusername/' + userName);
  }

  updateUser(userDetail : UserDetail){
    return this._httpClient.put<UserDetail>(this.baseUrl + 'user', userDetail);
  }

  getUsers(){
    return this._httpClient.get<UserDetail[]>(this.baseUrl + 'user');
  }

  setMainPhoto(photoId : number){
    return this._httpClient.put(this.baseUrl + 'user/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this._httpClient.delete(this.baseUrl + 'user/delete-photo/' + photoId);
  }
}
