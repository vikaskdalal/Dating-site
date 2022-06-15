import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MessageComponent } from './message/message.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AuthInterceptor } from './_interceptor/auth.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ErrorInterceptor } from './_interceptor/error.interceptor';
import { ServerErrorComponent } from './server-error/server-error.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserCardComponent } from './user-card/user-card.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    MessageComponent,
    EditUserComponent,
    ServerErrorComponent,
    UserListComponent,
    UserCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ToastrModule.forRoot({
      positionClass : 'toast-bottom-right'
    })
  ],
  providers: [
    {provide : HTTP_INTERCEPTORS, useClass : AuthInterceptor, multi : true},
    {provide : HTTP_INTERCEPTORS, useClass : ErrorInterceptor, multi : true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
