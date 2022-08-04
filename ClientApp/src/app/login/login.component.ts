import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginModel: any = {};
  
  constructor(public accountService: AccountService, private _router: Router) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(user => {
      if(user)
        this._router.navigateByUrl('/friends');
    })
  }

  login() {
    this.accountService.login(this.loginModel).subscribe(res => {
      this._router.navigateByUrl('/friends');
      console.log(res);
    })
  }

}
