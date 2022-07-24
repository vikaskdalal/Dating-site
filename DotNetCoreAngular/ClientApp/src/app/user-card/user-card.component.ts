import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserDetail } from '../_models/userDetail';
import { LikeService } from '../_services/like.service';
import { PresenceService } from '../_services/presence.service';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user! : UserDetail | undefined;
  @Input() showLikeButton : boolean = true;
  onlineUsers : string[] = [];

  constructor(private _likeService : LikeService, private _toastr : ToastrService, private _presenceService : PresenceService) { }

  ngOnInit(): void {
    this.loadOnlineUsers();
  }

  loadOnlineUsers(){
    this._presenceService.onlineUsers$.subscribe(u => {
      this.onlineUsers = u;
    });
  }

  addLike(userDetail : UserDetail | undefined){
    this._likeService.addLike(userDetail?.username as string).subscribe(() => {
        this._toastr.success('You have liked ' + userDetail?.name);
    })
  }
}
