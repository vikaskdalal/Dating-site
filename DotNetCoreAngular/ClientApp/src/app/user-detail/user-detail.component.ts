import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Gender } from '../_enums/gender';
import { SelectDropDown } from '../_models/selectDropdown';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})

export class UserDetailComponent implements OnInit {
  @ViewChild('editForm') editForm! : NgForm;
  userDetail!: UserDetail;
  user! : User | null;
  gender = Gender;
  genderDropdown : SelectDropDown[] = [];
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event : any){
    if(this.editForm?.dirty)
      $event.returnValue = true;
  }

  constructor(private _userService : UserService, private _accountService : AccountService, private _toastr : ToastrService) { 
    this._accountService.currentUser$.subscribe(user => this.user = user);
  }
  

  ngOnInit(): void {
    this.loadUser();
    this.createGenderDropdown();
  }

  createGenderDropdown(){
    let keys = Object.values(this.gender).map((key :any) => this.gender[key]).filter(value => typeof value === 'string') as string[];
    
    keys.forEach((element :any)=> {
      this.genderDropdown.push({'value' : element ,'id' : this.gender[element]});
     
    });
  }

  loadUser(){
    this._userService.getUser(this.user?.userName).subscribe(
      user =>{
        this.userDetail = user
      },
      error =>{
        this._toastr.error('Could not fetch data. Please try again after some time.')
        console.log(error)
      }
      );
  }

  update(){
    this._userService.updateUser(this.userDetail).subscribe(u => 
      {
        console.log(u)
        this._toastr.success("Profile has been updated");
      }
    
    );
    console.log(this.userDetail);
  }

}
