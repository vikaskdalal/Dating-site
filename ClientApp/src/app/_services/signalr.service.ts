import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { BehaviorSubject, Subject } from 'rxjs';
import { CallNotification } from '../_models/callNotification';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  hubUrl = environment.hubUrl;
  private _hubConnection!: HubConnection;

  private _onlineUserSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this._onlineUserSource.asObservable();

  private callNotificationSource = new Subject<CallNotification>();
  callNotification$ = this.callNotificationSource.asObservable();

  constructor(private _toastrService: ToastrService) { }

  createHubConnection(user: User) {
    this._hubConnection = new HubConnectionBuilder().
      withUrl(this.hubUrl + 'signalR', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this._hubConnection
      .start()
      .catch(error => {
        console.log(error)
      });

    this._hubConnection.on("GetOnlineUsers", (users: string[]) => {
      this._onlineUserSource.next(users);
    })

    this._hubConnection.on('ReceiveCallNotification', response => {
      this.callNotificationSource.next(response);
    })

  }

  async sendCallNotification(friendUsername: string, callType: string, callerName: string){
    return this._hubConnection.invoke("SendCallNotification", friendUsername, callType, callerName);
  }

  async sendCallResponse(callerConnectionId: string, callResponse: string){
    return this._hubConnection.invoke("SendCallResponse", callerConnectionId, callResponse);
  }

  stopHubConnection() {
    if (this._hubConnection)
      this._hubConnection.stop().catch(error => console.log(error));
  }
}
