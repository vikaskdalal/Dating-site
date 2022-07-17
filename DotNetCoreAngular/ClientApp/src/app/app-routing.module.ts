import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditUserComponent } from './edit-user/edit-user.component';
import { HomeComponent } from './home/home.component';
import { MessageComponent } from './message/message.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserLikeComponent } from './user-like/user-like.component';
import { UserListComponent } from './user-list/user-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'user/edit', component: EditUserComponent, canActivate : [AuthGuard], canDeactivate : [PreventUnsavedChangesGuard]},
  {path: 'friends', component: UserListComponent, canActivate : [AuthGuard]},
  {path: 'messages', component: MessageComponent},
  {path: 'user/:username', component: UserDetailComponent, canActivate : [AuthGuard]},
  {path: 'user-likes', component: UserLikeComponent, canActivate : [AuthGuard]},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
