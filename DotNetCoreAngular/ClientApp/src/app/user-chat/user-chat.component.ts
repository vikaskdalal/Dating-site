import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, fromEvent } from 'rxjs';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit, AfterViewInit, OnDestroy{
  
  @ViewChild('messageForm') messageForm! : NgForm;
  @ViewChild('chatBox') chatBox!: ElementRef;
  @ViewChild('chatContainer') chatContainer!: ElementRef;
  @ViewChild('chatList') chatList!: ElementRef;
  friendUsername! : string;
  messageContent!: string;
  sentTypingEvent : boolean = true;
  isRecipientTyping : boolean = false;
  user! : User;
  friendDetails! : UserDetail;
  chatPagination!: Pagination;

  constructor(public messageService : MessageService, private _route : ActivatedRoute, 
    private _userService : UserService, private _accountService : AccountService, public presenceService : PresenceService) { 
    
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
     this.loadChatPagination();
  }

  loadFriendsDetails(){
    this._userService.getByUsername(this.friendUsername).subscribe(user => this.friendDetails = user);
  }

  loadChatPagination(){
    this.messageService.messageThreadPagination$.subscribe(response => this.chatPagination = response!);
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

  onScrollUp(ev: any) {
    let scrollTop = this.chatContainer.nativeElement.scrollTop;
    let chatContainerOffsetHeight = this.chatContainer.nativeElement.offsetHeight;
    let chatlistOffsetHeight = this.chatList.nativeElement.offsetHeight;
    let pagination = this.chatPagination;
    
    if(chatContainerOffsetHeight-scrollTop >= chatlistOffsetHeight && pagination.currentPage != pagination.totalPages){
        console.log("more chat loaded");
        this.messageService.loadMessageThreadOnScroll(this.friendUsername, this.chatPagination).then(()=>{
          this.chatContainer.nativeElement.scrollTop = scrollTop;
        })
    }
    
  }

}
