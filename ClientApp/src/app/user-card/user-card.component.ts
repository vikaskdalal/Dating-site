import { Component, Input, OnInit } from '@angular/core';
import { UserDetail } from '../_models/userDetail';
import { LikeService } from '../_services/like.service';
import { SignalRService } from '../_services/signalr.service';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user!: UserDetail | undefined;
  @Input() showLikeButton: boolean = true;
  onlineUsers: string[] = [];

  constructor(
    private _likeService: LikeService,
    private _signalrService: SignalRService) { }

  ngOnInit(): void {
    this.loadOnlineUsers();
  }

  loadOnlineUsers() {
    this._signalrService.onlineUsers$.subscribe(u => {
      this.onlineUsers = u;
    });
  }

  addLike(userDetail: UserDetail | undefined) {
    this._likeService.addLike(userDetail?.username as string).subscribe(() => {
      this.showLikeButton = false;
    })
  }

  removeLike(userDetail: UserDetail | undefined) {
    this._likeService.removeLike(userDetail?.username as string).subscribe(() => {
      this.showLikeButton = true;
    })
  }
}
