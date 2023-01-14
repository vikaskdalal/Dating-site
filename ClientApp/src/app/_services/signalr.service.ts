import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { BehaviorSubject, Subject } from 'rxjs';
import { CallNotification } from '../_models/callNotification';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  hubUrl = environment.hubUrl;
  apiUrl = environment.apiUrl;
  private _hubConnection!: HubConnection;

  private _onlineUserSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this._onlineUserSource.asObservable();

  private callNotificationSource = new Subject<CallNotification>();
  callNotification$ = this.callNotificationSource.asObservable();

  private _hubConnectionState = new BehaviorSubject<boolean>(false);
  hubConnectionState$ = this._hubConnectionState.asObservable();

  constructor(private _httpClient: HttpClient) { }

  createHubConnection(user: User): Promise<any> {
    this._hubConnection = new HubConnectionBuilder().
      withUrl(this.hubUrl + 'signalR', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();
    
    this.registerEvents();
    
    return this._hubConnection
      .start()
      .catch(error => {
        console.log(error)
      });
  }

  changeHubState(state: boolean){
    this._hubConnectionState.next(state);
  }

  private registerEvents(){
    this._hubConnection.on("GetOnlineUsers", (users: string[]) => {
      this._onlineUserSource.next(users);
    })

    this._hubConnection.on('ReceiveCallNotification', response => {
      this.callNotificationSource.next(response);
    })
  }

  async sendCallNotification(friendUsername: string, callType: string){
    return this._hubConnection?.invoke("SendCallNotification", friendUsername, callType);
  }

  async sendResponse(callerConnectionId: string, response: string, data?: any){
    return this._hubConnection?.invoke("SendCallResponse", callerConnectionId, response, data);
  }

  stopHubConnection() {
    if (this._hubConnection)
      this._hubConnection.stop().catch(error => console.log(error));
  }
}
