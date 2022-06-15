import { Component, OnInit } from '@angular/core';
import { UserDetail } from '../_models/userDetail';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users! : UserDetail[];
  constructor(private _userService : UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(){
    this._userService.getUsers().subscribe(users =>{
        this.users = users;
      })
  }

}
