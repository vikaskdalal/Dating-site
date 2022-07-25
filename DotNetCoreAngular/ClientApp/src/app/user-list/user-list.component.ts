import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { LikeService } from '../_services/like.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users! : UserDetail[];
  currentUser! : User;
  userLikedByme : string[] = [];

  constructor(private _userService : UserService, private _accountService : AccountService, private _likeService : LikeService) { 
    this._accountService.currentUser$.subscribe(user => {
      if(user != null)
        this.currentUser = user;
    })
  }

  ngOnInit(): void {
    this.loadUsers();
    this.getUserWhoLikedByMe();
  }

  loadUsers(){
    this._userService.getUsers().subscribe(users =>{
        this.users = users.filter(f => f.username != this.currentUser.username);
      })
  }

  getUserWhoLikedByMe(){
    this._likeService.getUserLikedByMe().subscribe(data =>{
      this.userLikedByme = data.map(u => {
          return u?.username!
      });
    })
  }

}
