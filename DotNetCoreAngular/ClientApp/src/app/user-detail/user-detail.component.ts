import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from '../_models/message';
import { UserDetail } from '../_models/userDetail';
import { MessageService } from '../_services/message.service';
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

  constructor(private _userService : UserService, private _route : ActivatedRoute, private _messageService : MessageService) { }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(){
    this._userService.getByUsername(this._route.snapshot.paramMap.get('username')).subscribe(data => {
      this.userDetails = data;
    })
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
      this.loadMessages();
    }
  }

}
