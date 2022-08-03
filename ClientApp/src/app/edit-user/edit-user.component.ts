import { AfterViewInit, Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { Gender } from '../_enums/gender';
import { SelectDropDown } from '../_models/selectDropdown';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit, AfterViewInit {

  @ViewChild('editForm') editForm!: NgForm;
  userDetail!: UserDetail;
  user!: User | null;
  gender = Gender;
  genderDropdown: SelectDropDown[] = [];
  isUsernameAvailable: boolean = true;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty)
      $event.returnValue = true;
  }

  constructor(private _userService: UserService, private _accountService: AccountService,
    private _toastr: ToastrService, private _titleService: Title) {
    this._accountService.currentUser$.subscribe(user => this.user = user);
  }


  ngOnInit(): void {
    this.loadUser();
    this.createGenderDropdown();
  }

  ngAfterViewInit(): void {
    this._titleService.setTitle("User Profile - " + this.user?.name);
  }

  createGenderDropdown() {
    let keys = Object.values(this.gender).map((key: any) => this.gender[key]).filter(value => typeof value === 'string') as string[];

    keys.forEach((element: any) => {
      this.genderDropdown.push({ 'value': element, 'id': this.gender[element] });

    });
  }

  loadUser() {
    this._userService.getUser(this.user?.email).subscribe(
      user => {
        this.userDetail = user
      },
      error => {
        this._toastr.error('Could not fetch data. Please try again after some time.')
        console.log(error)
      }
    );
  }

  update(form: NgForm) {
    this._userService.updateUser(this.userDetail).subscribe(u => {
      console.log(u)
      form.form.markAsPristine();
      form.form.markAsTouched();
      this._toastr.success("Profile has been updated");
    }

    );
    console.log(this.userDetail);
  }

  checkUsername(username: string) {
    if (username == this.user?.username)
      return;

    this._userService.getByUsername(username).subscribe(data => {
      if (data == null)
        this.isUsernameAvailable = true;
      else
        this.isUsernameAvailable = false;
    });
    console.log(username);
  }

}
