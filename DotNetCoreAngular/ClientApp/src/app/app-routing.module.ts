import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MessageComponent } from './message/message.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'user/edit', component: UserDetailComponent, canActivate : [AuthGuard], canDeactivate : [PreventUnsavedChangesGuard]},
  {path: 'lists', component: ListComponent, canActivate : [AuthGuard]},
  {path: 'messages', component: MessageComponent, canActivate : [AuthGuard]},
  {path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
