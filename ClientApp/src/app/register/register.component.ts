import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(private _accountService: AccountService, private _router: Router) { }

  ngOnInit(): void {
    this._accountService.currentUser$.subscribe(user => {
      if(user)
        this._router.navigateByUrl('/friends');
    })
  }

  register() {
    this._accountService.register(this.model).subscribe(response => {
      console.log(response);
    })
  }
}
