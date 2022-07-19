import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';

  constructor(private _accountService : AccountService, private datePipe : DatePipe){}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user : User = JSON.parse(localStorage.getItem('user') || '{}');
    
    if(user.token != undefined){
      if(this._accountService.isTokenExpired(user)){
        this._accountService.logout();
      }
      else{
        this._accountService.setCurrentUser(user);
      }
    }
  }
}
