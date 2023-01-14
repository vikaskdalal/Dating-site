import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { SignalRService } from './_services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  user!: User;
  constructor(
    private _accountService : AccountService, 
    private _signalrService : SignalRService
    ){}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    this.user = JSON.parse(localStorage.getItem('user') || '{}');
    
    if(this.user.token == undefined || this._accountService.isTokenExpired(this.user)){
      this._accountService.logout();
      return;
    }
    
    this._accountService.setCurrentUser(this.user);
    this._signalrService.createHubConnection(this.user)
    .then(()=>{
      this._signalrService.changeHubState(true);
    });
  }
}
