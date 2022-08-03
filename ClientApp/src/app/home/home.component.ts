import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: boolean = false;
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = true;
  }

  cancelRegisterEvent(event: boolean) {
    this.registerMode = event;
  }

}
