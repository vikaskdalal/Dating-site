import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private _hubConnection!: HubConnection;

  private _onlineUserSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this._onlineUserSource.asObservable();

  constructor(private _toastrService : ToastrService) { }

  createHubConnection(user : User){
    this._hubConnection = new HubConnectionBuilder().
    withUrl(this.hubUrl + 'presence', {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();

    this._hubConnection
    .start()
    .catch(error => {
      console.log(error)
    });

    this._hubConnection.on('UserIsOnline', username => {
      this._toastrService.info(username + ' is online')
    });

    this._hubConnection.on('UserIsOffline', username => {
      this._toastrService.warning(username + ' is offline')
    })

    this._hubConnection.on("GetOnlineUsers", (users : string[]) => {
      this._onlineUserSource.next(users);
    })
    
  }

  stopHubConnection(){
    this._hubConnection.stop().catch(error => console.log(error));
  }
}
