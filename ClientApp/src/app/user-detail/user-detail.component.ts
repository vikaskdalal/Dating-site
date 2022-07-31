import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { UserDetail } from '../_models/userDetail';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  userDetails! : UserDetail;

  constructor(private _userService : UserService, private _route : ActivatedRoute, 
    public presenceService : PresenceService, private _titleService : Title) 
    { 
    }
  ngOnInit(): void {
    this.loadUser();
    this._titleService.setTitle("User Details");
  }

  loadUser(){
    this._userService.getByUsername(this._route.snapshot.paramMap.get('username')).subscribe(data => {
      this.userDetails = data;
    })
  }
}
