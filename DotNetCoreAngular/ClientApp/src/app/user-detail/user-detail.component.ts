import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  constructor(private _userService : UserService, private _route : ActivatedRoute) { }
  user : UserDetail | undefined;

  ngOnInit(): void {
    this._userService.getUser(this._route.snapshot.paramMap.get('username')).subscribe(user => 
      {
        this.user = user
        console.log(user);
      }
      );
  }

}
