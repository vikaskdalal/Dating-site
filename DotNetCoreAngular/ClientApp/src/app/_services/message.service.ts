import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  
  constructor(private _httpClient : HttpClient) { }

  getMessages(pageNumber : number, pageSize : number, container : string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'message', params, this._httpClient);
  }

  getMessageThread(username : string){
    return this._httpClient.get<Message[]>(this.baseUrl + 'message/thread/'+ username);
  }

  sendMessage(username: string, content: string) {
    return this._httpClient.post<Message>(this.baseUrl + 'message', {recipientUsername: username, content})
  }

  deleteMessage(id : number){
    return this._httpClient.delete(this.baseUrl+ 'message/' + id);
  }
}
