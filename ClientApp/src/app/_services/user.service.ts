import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { UserDetail } from '../_models/userDetail';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<UserDetail[]> = new PaginatedResult<UserDetail[]>();

  constructor(private _httpClient: HttpClient) { }

  getUser(email: string | undefined) {
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/' + email);
  }

  getByUsername(userName: string | null) {
    return this._httpClient.get<UserDetail>(this.baseUrl + 'user/getbyusername/' + userName);
  }

  updateUser(userDetail: UserDetail) {
    return this._httpClient.put<UserDetail>(this.baseUrl + 'user', userDetail);
  }

  getUsers(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this._httpClient.get<UserDetail[]>(this.baseUrl + 'user', { observe: 'response', params }).pipe(
      map(response => {
        if (response.body)
          this.paginatedResult.result = response.body;

        let paginationHeader = response.headers.get('Pagination');
        if (paginationHeader != null) {
          this.paginatedResult.pagination = JSON.parse(paginationHeader)
        }
        return this.paginatedResult;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this._httpClient.put(this.baseUrl + 'user/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this._httpClient.delete(this.baseUrl + 'user/delete-photo/' + photoId);
  }
}
