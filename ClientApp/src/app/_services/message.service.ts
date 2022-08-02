import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { TrackMessageThread } from '../_models/trackMessageThread';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/user';
import { UserTyping } from '../_models/userTyping';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  private _hubConnection!: HubConnection;
  hubUrl = environment.hubUrl;

  private _messageSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this._messageSource.asObservable();

  private _showRecipientIsTypingSource = new BehaviorSubject<UserTyping[]>([]);
  recipientIsTypingSource$ = this._showRecipientIsTypingSource.asObservable();

  private _trackMessageThreadSource = new BehaviorSubject<TrackMessageThread[]>([]);
  trackMessageThread$ = this._trackMessageThreadSource.asObservable();

  constructor(private _httpClient: HttpClient) { }

  createHubConnection(user: User, otherUser: string) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUser + '&skipMessage=0&takeMessage=10', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this._hubConnection.start().catch(error => console.log(error));

    this._hubConnection.on('ReceiveMessageThread', response => {
      this._trackMessageThreadSource.next([response.trackMessageThread]);
      this._messageSource.next(response.messages);

    })

    this._hubConnection.on('ReceiveMessageThreadOnScroll', response => {

      this.trackMessageThread$.pipe(take(1)).subscribe(res => {
        let trackingInfoOfUser = res.filter(f => f.friendUsername != response.trackMessageThread.friendUsername);

        this._trackMessageThreadSource.next([...trackingInfoOfUser, response.trackMessageThread]);
      })

      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this._messageSource.next([...response.messages, ...messages]);
      })
    })

    this._hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this._messageSource.next([...messages, message]);
      })

      this.trackMessageThread$.pipe(take(1)).subscribe(response => {
        let trackingInfoOfUser = response.filter(f => f.friendUsername == message.recipientUsername);

        if (trackingInfoOfUser.length != 0) {
          trackingInfoOfUser[0].messageLoaded++;
          trackingInfoOfUser[0].totalMessages++;
          this._trackMessageThreadSource.next([...response, trackingInfoOfUser[0]]);
        }
      })

    })

    this._hubConnection.on('UserIsTyping', username => {
      let userIsTyping = new UserTyping(username, true);

      this.recipientIsTypingSource$.pipe(take(1)).subscribe(userList => {
        this._showRecipientIsTypingSource.next([...userList, userIsTyping]);
      })
    })

    this._hubConnection.on('UserHasStoppedTyping', username => {
      this.recipientIsTypingSource$.pipe(take(1)).subscribe(users => {
        this._showRecipientIsTypingSource.next(users.filter(f => f.username != username));
      })
    })
  }

  stopHubConnection() {
    if (this._hubConnection)
      this._hubConnection.stop();
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'message', params, this._httpClient);
  }

  async sendMessage(username: string, content: string) {
    return this._hubConnection.invoke("SendMessage", { recipientUsername: username, content })
  }

  deleteMessage(id: number) {
    return this._httpClient.delete(this.baseUrl + 'message/' + id);
  }

  deleteUserChat(recipientUsername : string){
    return this._httpClient.delete(this.baseUrl + 'message/delete-user-chat/' + recipientUsername);
  }

  clearChatMessageThread(){
    this.messageThread$.pipe(take(1)).subscribe(messages => {
      this._messageSource.next([]);
    })
  }

  async sendUserIsTypingEvent(username: string) {
    return this._hubConnection.invoke("UserIsTyping", username)
  }

  async sendUserHasStoppedTypingEvent(username: string) {
    return this._hubConnection.invoke("UserHasStoppedTyping", username)
  }

  async loadMessageThreadOnScroll(username: string, skipMessages: number, takeMessages: number) {
    return this._hubConnection.invoke("LoadMessageThreadOnScroll", { recipientUsername: username, skipMessages, takeMessages })
  }
}
