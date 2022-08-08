import {Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotificationType } from '../_common/notificationType';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { SignalRService } from '../_services/signalr.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-video-call',
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css']
})
export class VideoCallComponent implements OnInit{
  friendDetails!: UserDetail;
  friendUsername!: string;
  callType!: string;
  user!: User;

  constructor(
    private _route: ActivatedRoute,
    private _userService: UserService,
    private _signalrService: SignalRService,
    private _accountService: AccountService
  ) 
  { 
    this.friendUsername = this._route.snapshot.paramMap.get('username')!;
    this.callType = this._route.snapshot.paramMap.get('calltype')!;
    this._accountService.currentUser$.subscribe((user) => (this.user = user!));
  }

  ngOnInit(): void {
    this.loadFriendsDetails();
    // this._signalrService.createHubConnection(this.user, this.friendUsername).then(()=>{
    //   this._videoCallService.registerEvents();
    //   this.sendCallNotification();
    //   this.handleCallNotification();
    // });

      this.sendCallNotification();
      this.handleCallNotification();
  }

  handleCallNotification() {
    this._signalrService.callNotification$.subscribe((response) => {
      if (response.notificationType == NotificationType.CallRejected){
        this.handleCallRejected();
      } 
      else if (response.notificationType == NotificationType.CallAccepted) {
        alert('call accepted');
      }
    });
  }

  private handleCallRejected() {
    document.getElementById('calling_text')!.innerHTML = 'Call Rejected';
    setTimeout(() => {
      window.close();
    }, 2000);
  }

  loadFriendsDetails() {
    this._userService
      .getByUsername(this.friendUsername)
      .subscribe((user) => (this.friendDetails = user));
  }

  sendCallNotification(){
    this._signalrService
      .sendCallNotification(this.friendUsername, this.callType, this.user.username)
      .then(() => {
        
      });
  }

  cancelCall(){
    this._signalrService
    .sendCallNotification(
      this.friendUsername,
      NotificationType.CallCancelled,
      this.user.name
    )
    .then(() => {
      window.close();
    });
  }

}
