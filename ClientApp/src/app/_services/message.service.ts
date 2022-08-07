import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Subject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CallNotification } from '../_models/callNotification';
import { Message } from '../_models/message';
import { TrackMessageThread } from '../_models/trackMessageThread';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  private _hubConnection!: HubConnection;
  hubUrl = environment.hubUrl;
  loadMessageCount = environment.loadMessageCount;

  private _messageSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this._messageSource.asObservable();

  private _typingSourceSet = new Set<string>();
  private _recipientIsTypingSource = new BehaviorSubject<Set<string>>(new Set<string>());
  recipientIsTypingSource$ = this._recipientIsTypingSource.asObservable();

  private _trackMessageThreadSource = new BehaviorSubject<TrackMessageThread[]>([]);
  trackMessageThread$ = this._trackMessageThreadSource.asObservable();

  private callNotificationSource = new Subject<CallNotification>();
  callNotification$ = this.callNotificationSource.asObservable();

  constructor(private _httpClient: HttpClient) { }

  createHubConnection(user: User, otherUser: string) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUser + '&skipMessage=0&takeMessage=' + this.loadMessageCount, {
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

      this._messageSource.next([...response.messages, ...this._messageSource.getValue()]);
    })

    this._hubConnection.on('NewMessage', message => {
      this._messageSource.next([...this._messageSource.getValue(), message]);

      var getValue = this._trackMessageThreadSource.getValue();
      let trackingInfoOfUser = getValue.filter(f => f.friendUsername == message.recipientUsername);

      if (trackingInfoOfUser.length != 0) {
        trackingInfoOfUser[0].messageLoaded++;
        trackingInfoOfUser[0].totalMessages++;
        this._trackMessageThreadSource.next([...getValue, trackingInfoOfUser[0]]);
      }
    })

    this._hubConnection.on('UserIsTyping', (username: string) => {
      this._typingSourceSet.add(username);
      this._recipientIsTypingSource.next(this._typingSourceSet);
    })

    this._hubConnection.on('UserHasStoppedTyping', (username: string) => {
      this._typingSourceSet.delete(username);
      this._recipientIsTypingSource.next(this._typingSourceSet);
    })

    this._hubConnection.on('ReceiveCallNotification', response => {
      this.callNotificationSource.next(response);
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

  deleteUserChat(recipientUsername: string) {
    return this._httpClient.delete(this.baseUrl + 'message/delete-user-chat/' + recipientUsername);
  }

  clearChatMessageThread() {
    this._messageSource.next([]);
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

  async sendCallNotification(friendUsername: string, callType: string, callerName: string){
    return this._hubConnection.invoke("SendCallNotification", friendUsername, callType, callerName);
  }

  async sendCallResponse(callerConnectionId: string, callResponse: string){
    return this._hubConnection.invoke("SendCallResponse", callerConnectionId, callResponse);
  }
}
