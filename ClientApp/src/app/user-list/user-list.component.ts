import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { UserDetail } from '../_models/userDetail';
import { LikeService } from '../_services/like.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users!: UserDetail[];
  userLikedByme: string[] = [];

  pagination!: Pagination | null;
  pageNumber = 1;
  pageSize = 2;

  constructor(private _userService: UserService, private _likeService: LikeService) {
  }

  ngOnInit(): void {
    this.loadUsers();
    this.getUserWhoLikedByMe();
  }

  loadUsers() {
    this._userService.getUsers(this.pageNumber, this.pageSize).subscribe(response => {
      this.users = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadUsers();
  }

  getUserWhoLikedByMe() {
    this._likeService.getUserLikedByMe().subscribe(data => {
      this.userLikedByme = data.map(u => {
        return u?.username!
      });
    })
  }

}
