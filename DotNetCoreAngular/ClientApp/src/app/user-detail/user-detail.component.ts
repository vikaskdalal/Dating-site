import { Component, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  @ViewChild('userTabs') userTabs!: TabsetComponent;
  userDetails! : UserDetail;
  activeTab! : TabDirective;
  messages : Message[] = [];
  onlineUsers : string[] = [];
  user! : User;
  constructor(private _userService : UserService, private _route : ActivatedRoute,
              private _presenceService : PresenceService, private _accountService : AccountService, 
              private _titleService : Title) { 
                this._accountService.currentUser$.subscribe(user => {
                  if(user != null)
                    this.user = user
                });
              }
  ngOnInit(): void {
    this.loadUser();
    this.loadOnlineUsers();
    this._titleService.setTitle("User Details");
  }

  loadUser(){
    this._userService.getByUsername(this._route.snapshot.paramMap.get('username')).subscribe(data => {
      this.userDetails = data;
    })
  }

  loadOnlineUsers(){
    this._presenceService.onlineUsers$.subscribe(u => {
      this.onlineUsers = u;
    });
  }
}
