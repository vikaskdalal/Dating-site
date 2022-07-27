import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  private _hubConnection! : HubConnection;
  hubUrl = environment.hubUrl;
  private messageSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageSource.asObservable();

  constructor(private _httpClient : HttpClient) { }

  createHubConnection(user : User, otherUser : string){
    this._hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?user=' + otherUser,{
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();

    this._hubConnection.start().catch(error => console.log(error));

    this._hubConnection.on('ReceiveMessageThread', messages => {
      this.messageSource.next(messages);
    })

    this._hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageSource.next([...messages, message]);
      })
    })
  }

  stopHubConnection(){
    if(this._hubConnection)
      this._hubConnection.stop();
  }

  getMessages(pageNumber : number, pageSize : number, container : string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'message', params, this._httpClient);
  }

  getMessageThread(username : string){
    return this._httpClient.get<Message[]>(this.baseUrl + 'message/thread/'+ username);
  }

  async sendMessage(username: string, content: string) {
    return this._hubConnection.invoke("SendMessage", {recipientUsername: username, content})
  }

  deleteMessage(id : number){
    return this._httpClient.delete(this.baseUrl+ 'message/' + id);
  }
}
