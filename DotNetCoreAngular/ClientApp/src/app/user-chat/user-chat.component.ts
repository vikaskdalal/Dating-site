import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, fromEvent } from 'rxjs';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit, AfterViewInit, OnDestroy{
  
  @ViewChild('messageForm') messageForm! : NgForm;
  @ViewChild('chatBox') chatBox!: ElementRef;
  friendUsername! : string;
  messageContent!: string;
  sentTypingEvent : boolean = true;
  isRecipientTyping : boolean = false;
  user! : User;
  friendDetails! : UserDetail;

  constructor(public messageService : MessageService, private _route : ActivatedRoute, 
    private _userService : UserService, private _accountService : AccountService) { 
    
      let username = this._route.snapshot.paramMap.get('username');
    if(username)
      this.friendUsername =  username;

      this._accountService.currentUser$.subscribe(user => this.user = user!);
  }

  ngOnInit(): void {
    this.loadFriendsDetails();
  }

  ngAfterViewInit() {     
     this.sendEventWhenUserStopsTyping();
     this.checkIfRecipientTyping();
     this.messageService.createHubConnection(this.user, this.friendUsername);
  }

  loadFriendsDetails(){
    this._userService.getByUsername(this.friendUsername).subscribe(user => this.friendDetails = user);
  }

  ngOnDestroy(): void{
    this.messageService.stopHubConnection();
  }

  checkIfRecipientTyping(){
    this.messageService.recipientIsTypingSource$.subscribe(res => {
      this.isRecipientTyping = res.filter(f => f.username == this.friendUsername).length != 0;
     })
  }

  sendEventWhenUserStopsTyping(){
    fromEvent(this.chatBox.nativeElement, 'input')
    .pipe(debounceTime(1000))
    .subscribe(data => {
      this.messageService.sendUserHasStoppedTypingEvent(this.friendUsername).then(() => {
        this.sentTypingEvent = true;
      });
      
    });  
  }

  sendMessage() {
    this.messageService.sendMessage(this.friendUsername, this.messageContent).then(() => {
        this.messageForm.reset();
    });
  }

  onKeyUpEvent(event: any){
    if(!this.sentTypingEvent)
      return;

    this.messageService.sendUserIsTypingEvent(this.friendUsername).then(()=> {
      this.sentTypingEvent = false;
    });
  }

}
