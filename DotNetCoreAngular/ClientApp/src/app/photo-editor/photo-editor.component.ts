import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { Photo } from '../_models/photo';
import { User } from '../_models/user';
import { UserDetail } from '../_models/userDetail';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  
  @Input() userDetail! : UserDetail;

  uploader!: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user! : User | null;

  constructor(private _accountService : AccountService, private _userService : UserService, private _toastrService : ToastrService) { }

  ngOnInit(): void {
    this._accountService.currentUser$.subscribe(user => this.user = user);
    this.initializeUploader();

    let abc = this.userDetail;
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  setMainPhoto(photo : Photo){
    this._userService.setMainPhoto(photo.id).subscribe(() => {
      this._accountService.setCurrentUser(this.user);
      this.userDetail.photoUrl = photo.url;
      this.userDetail.photos.forEach(p => {
        if (p.isMain) p.isMain = false;
        if (p.id === photo.id) p.isMain = true;
      })
      this._toastrService.success("Main photo changed successfully.");
    });
  }

  deletePhoto(photoId: number) {
    this._userService.deletePhoto(photoId).subscribe(() => {
      this.userDetail.photos = this.userDetail.photos.filter(x => x.id !== photoId);
      this._toastrService.success("Photo deleted successfully.");
    })
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.userDetail.photos.push(photo);
         if (photo.isMain) {
           this.userDetail.photoUrl = photo.url;
           this.userDetail.photoUrl = photo.url;
           this._accountService.setCurrentUser(this.user);
         }
         this._toastrService.success("Photo uploaded successfully.");
      }
    }
  }

}
