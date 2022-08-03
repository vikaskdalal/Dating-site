import { Component, OnInit } from '@angular/core';
import { UserDetail } from '../_models/userDetail';
import { LikeService } from '../_services/like.service';
import { Title } from "@angular/platform-browser";

@Component({
  selector: 'app-user-like',
  templateUrl: './user-like.component.html',
  styleUrls: ['./user-like.component.css']
})
export class UserLikeComponent implements OnInit {

  users!: Partial<UserDetail[]>;
  constructor(private _likeService: LikeService, private _titleService: Title) { }

  ngOnInit(): void {
    this.getUserWhoLikeMe();
  }
  headingText: string = '';

  getUserWhoLikeMe() {
    this._likeService.getUserWhoLikeMe().subscribe(data => {
      this.users = data;
      this.headingText = "User Who Like Me";
      this._titleService.setTitle("User Who Like Me");
    })
  }

  getUserWhoLikedByMe() {
    this._likeService.getUserLikedByMe().subscribe(data => {
      this.users = data;
      this.headingText = "User Liked By Me";
      this._titleService.setTitle("User Liked By Me");
    })
  }

}
