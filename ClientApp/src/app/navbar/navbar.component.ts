import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  
  currentUsername?: string = undefined;
  isShown = false;

  constructor(public accountService: AccountService, private _router: Router) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(q => this.currentUsername = q?.email);
  }

  logout() {
    this.accountService.logout();
    this._router.navigateByUrl('/');
  }

}
