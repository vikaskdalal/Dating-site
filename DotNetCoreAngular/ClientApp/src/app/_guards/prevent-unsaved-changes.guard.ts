import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { UserDetailComponent } from '../user-detail/user-detail.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: UserDetailComponent): boolean {
      if(component.editForm?.dirty)
         return confirm('Are you sure you want leave ? Unsaved changes maybe lost.')
    return true;
  }
  
}
