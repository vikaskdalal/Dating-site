import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserDetail } from '../_models/userDetail';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.apiUrl;

  constructor(private _httpClient : HttpClient) { }

  addLike(username : string){
    return this._httpClient.post(this.baseUrl + 'like/' + username, {});
  }

  removeLike(username : string){
    return this._httpClient.delete(this.baseUrl + 'like/remove-like/' + username, {});
  }

  getUserWhoLikeMe(){
    return this._httpClient.get<Partial<UserDetail[]>>(this.baseUrl + 'like/like-me');
  }

  getUserLikedByMe(){
    return this._httpClient.get<Partial<UserDetail[]>>(this.baseUrl + 'like/liked-by-me');
  }

}
