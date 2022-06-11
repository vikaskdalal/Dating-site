import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  loginModel : any = {};
  currentUsername? : string = undefined;

  constructor(public accountService : AccountService, private router: Router) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(q => this.currentUsername = q?.userName);
  }

  login(){
    this.accountService.login(this.loginModel).subscribe(res => {
      this.router.navigateByUrl('/members');
      console.log(res);
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
