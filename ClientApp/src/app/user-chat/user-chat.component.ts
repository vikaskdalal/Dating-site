import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, fromEvent, take } from 'rxjs';
import { TrackMessageThread } from '../_models/trackMessageThread';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { ConfirmService } from '../_services/confirm.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('messageForm') messageForm!: NgForm;
  @ViewChild('chatBox') chatBox!: ElementRef;
  @ViewChild('chatContainer') chatContainer!: ElementRef;
  @ViewChild('chatList') chatList!: ElementRef;
  friendUsername!: string;
  messageContent!: string;
  sentTypingEvent: boolean = true;
  isRecipientTyping: boolean = false;
  user!: User;
  friendDetails!: UserDetail;
  trackChat!: TrackMessageThread;
  showChatDate: boolean = false;
  private keyCodeToSkipTypingEvent: number[] = [13];

  constructor(public messageService: MessageService, private _route: ActivatedRoute,
    private _userService: UserService, private _accountService: AccountService, public presenceService: PresenceService,
    private _confirmService: ConfirmService, private _toastrService: ToastrService) {

    let username = this._route.snapshot.paramMap.get('username');
    if (username)
      this.friendUsername = username;

    this._accountService.currentUser$.subscribe(user => this.user = user!);
  }

  ngOnInit(): void {
    this.loadFriendsDetails();
    this.messageService.createHubConnection(this.user, this.friendUsername);
  }

  ngAfterViewInit() {
    this.sendEventWhenUserStopsTyping();
    this.checkIfRecipientTyping();
    this.loadChatPagination();
  }

  loadFriendsDetails() {
    this._userService.getByUsername(this.friendUsername).subscribe(user => this.friendDetails = user);
  }

  loadChatPagination() {
    this.messageService.trackMessageThread$.subscribe(response => this.trackChat = response.filter(f => f.friendUsername ==
      this.friendUsername)[0]);
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  checkIfRecipientTyping() {
    this.messageService.recipientIsTypingSource$.subscribe(res => {
      this.isRecipientTyping = res.filter(f => f.username == this.friendUsername).length != 0;
    })
  }

  sendEventWhenUserStopsTyping() {
    fromEvent(this.chatBox.nativeElement, 'input')
      .pipe(debounceTime(1000))
      .subscribe(data => {
        this.messageService.sendUserHasStoppedTypingEvent(this.friendUsername).then(() => {
          this.sentTypingEvent = true;
        });

      });
  }

  sendMessage() {
    if (this.messageContent == undefined || this.messageContent.trim() == '') {
      this.messageContent = '';
      return;
    }

    this.messageService.sendMessage(this.friendUsername, this.messageContent).then(() => {
      this.messageForm.reset();
    });
  }

  onKeyUpEvent(event: any) {
    if (!this.sentTypingEvent || this.keyCodeToSkipTypingEvent.includes(event.keyCode))
      return;

    this.messageService.sendUserIsTypingEvent(this.friendUsername).then(() => {
      this.sentTypingEvent = false;
    });
  }

  onScrollUp(ev: any) {
    let scrollTop = this.chatContainer.nativeElement.scrollTop;
    let chatContainerOffsetHeight = this.chatContainer.nativeElement.offsetHeight;
    let chatlistOffsetHeight = this.chatList.nativeElement.offsetHeight;
    let scroll = Math.ceil(chatContainerOffsetHeight - scrollTop);

    if (scroll == chatlistOffsetHeight && this.trackChat.messageLoaded < this.trackChat.totalMessages) {
      this.messageService.loadMessageThreadOnScroll(this.friendUsername, this.trackChat.messageLoaded, 10).then(() => {
        this.chatContainer.nativeElement.scrollTop = scrollTop + 1;
      })
    }

  }

  clearChat(){
      this._confirmService.confirm().subscribe(result => {
        if(result){
            this.messageService.deleteUserChat(this.friendUsername).subscribe(() => {
              this._toastrService.success("Chat deleted.");
              this.messageService.clearChatMessageThread();
            })
        }
          
      })
  }

}
