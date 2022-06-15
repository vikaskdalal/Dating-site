import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  loginModel : any = {};
  currentUsername? : string = undefined;

  constructor(public _accountService : AccountService, private _router: Router) { }

  ngOnInit(): void {
    this._accountService.currentUser$.subscribe(q => this.currentUsername = q?.email);
  }

  login(){
    this._accountService.login(this.loginModel).subscribe(res => {
      this._router.navigateByUrl('/members');
      console.log(res);
    })
  }

  logout(){
    this._accountService.logout();
    this._router.navigateByUrl('/');
  }

}
