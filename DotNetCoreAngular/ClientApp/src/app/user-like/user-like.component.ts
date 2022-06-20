import { Component, OnInit } from '@angular/core';
import { UserDetail } from '../_models/userDetail';
import { LikeService } from '../_services/like.service';

@Component({
  selector: 'app-user-like',
  templateUrl: './user-like.component.html',
  styleUrls: ['./user-like.component.css']
})
export class UserLikeComponent implements OnInit {

  users! : Partial<UserDetail[]>;
  constructor(private _likeService : LikeService) { }

  ngOnInit(): void {
    this.getUserWhoLikeMe();
  }

  getUserWhoLikeMe(){
    this._likeService.getUserWhoLikeMe().subscribe(data =>{
      this.users = data;
    })
  }

  getUserWhoLikedByMe(){
    this._likeService.getUserLikedByMe().subscribe(data =>{
      this.users = data;
    })
  }

}
