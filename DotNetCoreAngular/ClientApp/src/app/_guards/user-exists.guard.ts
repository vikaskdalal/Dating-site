import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { UserService } from '../_services/user.service';

@Injectable({
  providedIn: 'root'
})
export class UserExistsGuard implements CanActivate {

  constructor(private _userService : UserService, private _toastr: ToastrService, private _router: Router){}
  
  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this._userService.getByUsername(route.params['username']).pipe(
      map(user => {
        if (user)
          return true;

        this._toastr.warning("User does not exists.");
        this._router.navigate(['/friends']);
        return false;
      }));
  }
  
}
