import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit, OnDestroy {
  @ViewChild('userTabs') userTabs!: TabsetComponent;
  userDetails! : UserDetail;
  activeTab! : TabDirective;
  messages : Message[] = [];
  onlineUsers : string[] = [];
  user! : User;
  constructor(private _userService : UserService, private _route : ActivatedRoute, private _messageService : MessageService,
              private _presenceService : PresenceService, private _accountService : AccountService) { 
                this._accountService.currentUser$.subscribe(user => {
                  if(user != null)
                    this.user = user
                });
              }
  ngOnInit(): void {
    this.loadUser();
    this.loadOnlineUsers();
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

  loadMessages(){
    this._messageService.getMessageThread(this.userDetails.username).subscribe(msg => {
      this.messages = msg;
    })
  }

  selectTab(tabId: number) {
    this.userTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading == 'Messages' && this.messages.length === 0){
      this._messageService.createHubConnection(this.user, this.userDetails.username);
    }
    else{
      this._messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {
    this._messageService.stopHubConnection();
  }

}
