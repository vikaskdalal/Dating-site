import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { EditUserComponent } from '../edit-user/edit-user.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: EditUserComponent): boolean {
    if (component.editForm?.dirty)
      return confirm('Are you sure you want leave ? Unsaved changes maybe lost.')
    return true;
  }

}
