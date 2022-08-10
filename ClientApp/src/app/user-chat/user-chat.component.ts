import {
  AfterViewChecked,
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, fromEvent, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { TrackMessageThread } from '../_models/trackMessageThread';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { ConfirmService } from '../_services/confirm.service';
import { MessageService } from '../_services/message.service';
import { SignalRService } from '../_services/signalr.service';
import { UserService } from '../_services/user.service';
import { NotificationType } from '../_common/notificationType';
import { CallNotification } from '../_models/callNotification';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css'],
})
export class UserChatComponent
  implements OnInit, AfterViewInit, AfterViewChecked, OnDestroy
{
  @ViewChild('messageForm') messageForm!: NgForm;
  @ViewChild('chatBox') chatBox!: ElementRef;
  @ViewChild('chatContainer') chatContainer!: ElementRef;
  @ViewChild('chatList') chatList!: ElementRef;

  friendUsername!: string;
  messageContent!: string;
  sentTypingEvent: boolean = true;
  user!: User;
  friendDetails!: UserDetail;
  trackChat!: TrackMessageThread;
  showChatDate: boolean = false;
  loadMessageCount = environment.loadMessageCount;
  messageLoading: boolean = false;
  showSearchContainer = false;
  searchResult: Array<{ id: string; text: string; messageDate: string }> = [];
  searchValue = '';
  videoCallType = NotificationType.VideoCall;
  audioCallType = NotificationType.AudioCall;
  showCallingWindow = false;
  callWindow: any;

  private keyCodeToSkipTypingEvent: number[] = [13];

  constructor(
    public messageService: MessageService,
    private _route: ActivatedRoute,
    private _userService: UserService,
    private _accountService: AccountService,
    public presenceService: SignalRService,
    private _confirmService: ConfirmService,
    private _toastrService: ToastrService,
    private _title: Title,
    private _signalrService: SignalRService,
  ) {
    let username = this._route.snapshot.paramMap.get('username');
    if (username) this.friendUsername = username;

    this._accountService.currentUser$.subscribe((user) => (this.user = user!));
  }

  ngOnInit(): void {
    this.loadFriendsDetails();
    this.messageService.createHubConnection(this.user, this.friendUsername).then(() => {
    });
  }

  ngAfterViewInit() {
    this.sendEventWhenUserStopsTyping();
    this.loadChatPagination();
  }

  ngAfterViewChecked(): void {
    this._title.setTitle('Chat with ' + this.friendDetails?.name);
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
    this.messageService.clearChatMessageThread();
  }

  loadFriendsDetails() {
    this._userService
      .getByUsername(this.friendUsername)
      .subscribe((user) => (this.friendDetails = user));
  }

  loadChatPagination() {
    this.messageService.trackMessageThread$.subscribe((response) => {
      this.trackChat = response.filter(
        (f) => f.friendUsername == this.friendUsername
      )[0];
    });
  }

  sendEventWhenUserStopsTyping() {
    fromEvent(this.chatBox.nativeElement, 'input')
      .pipe(debounceTime(1000))
      .subscribe((data) => {
        this.messageService
          .sendUserHasStoppedTypingEvent(this.friendUsername)
          .then(() => {
            this.sentTypingEvent = true;
          });
      });
  }

  sendMessage() {
    if (this.messageContent == undefined || this.messageContent.trim() == '') {
      this.messageContent = '';
      return;
    }

    this.messageService
      .sendMessage(this.friendUsername, this.messageContent)
      .then(() => {
        this.messageForm.reset();
      });
  }

  onKeyUpEvent(event: any) {
    if (
      !this.sentTypingEvent ||
      this.keyCodeToSkipTypingEvent.includes(event.keyCode)
    )
      return;

    this.messageService.sendUserIsTypingEvent(this.friendUsername).then(() => {
      this.sentTypingEvent = false;
    });
  }

  onScrollUp(ev: any) {
    let scrollTop = this.chatContainer.nativeElement.scrollTop;
    let chatlistOffsetHeight = this.chatList.nativeElement.offsetHeight;
    let scroll = Math.ceil(
      this.chatContainer.nativeElement.offsetHeight - scrollTop
    );

    if (
      scroll == chatlistOffsetHeight &&
      this.trackChat.messageLoaded < this.trackChat.totalMessages
    ) {
      this.messageLoading = true;
      this.messageService
        .loadMessageThreadOnScroll(
          this.friendUsername,
          this.trackChat.messageLoaded,
          this.loadMessageCount
        )
        .then(() => {
          this.chatContainer.nativeElement.scrollTop = scrollTop;
          this.messageLoading = false;
        });
    }
  }

  clearChat() {
    this._confirmService.confirm().subscribe((result) => {
      if (result) {
        this.messageService
          .deleteUserChat(this.friendUsername)
          .subscribe(() => {
            this._toastrService.success('Chat deleted.');
            this.messageService.clearChatMessageThread();
          });
      }
    });
  }

  trackByMessageId(index: any, item: Message) {
    return item.id;
  }

  showSearchBox() {
    this.showSearchContainer = !this.showSearchContainer;
  }

  searchInChat(search: any) {
    this.searchResult = [];

    if (this.searchValue.trim() == '') return;

    var slides = document.getElementsByClassName('chat-message');
    for (var i = 0; i < slides.length; i++) {
      let item = slides.item(i);
      if (item != null) {
        let message = item?.textContent!;
        if (
          message.toLowerCase().indexOf(this.searchValue.toLowerCase()) > -1
        ) {
          this.searchResult.push({
            id: item.id,
            text: message,
            messageDate: item.getAttribute('message-date')!,
          });
        }
      }
    }
  }

  scrollToMessage(id: string) {
    const element = document.getElementById(id);
    element?.classList.add('highlight');
    document.getElementById(id)?.scrollIntoView({
      behavior: 'smooth',
    });
    setTimeout(() => {
      element?.classList.remove('highlight');
    }, 1500);
  }

  callFriend(callType: string) {
    const requestVideo = callType == NotificationType.VideoCall;
    this.callWindow = window.open('./call/'+this.friendUsername+'?requestVideo='+requestVideo+'&outgoingCall=true',
     '_blank', "toolbar=no,scrollbars=no,resizable=no,width=500,height=720,left=150");
  }
}
