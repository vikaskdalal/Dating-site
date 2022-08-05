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
import { UserDetailComponent } from './user-detail/user-detail.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { UserLikeComponent } from './user-like/user-like.component';
import { PhotoEditorComponent } from './photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { DatePipe } from '@angular/common';
import { UserChatComponent } from './user-chat/user-chat.component';
import { TimeAgoPipe } from './_pipes/time-ago.pipe';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ChatDatePipe } from './_pipes/chat-date.pipe';
import { ModalComponent } from './modal/modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { LoginComponent } from './login/login.component';
import { HighlightSearchPipe } from './_pipes/highlight-search.pipe';

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
    UserCardComponent,
    UserDetailComponent,
    UserLikeComponent,
    PhotoEditorComponent,
    UserChatComponent,
    TimeAgoPipe,
    ChatDatePipe,
    ModalComponent,
    LoginComponent,
    HighlightSearchPipe
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
    }),
    TabsModule.forRoot(),
    FileUploadModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [
    DatePipe,
    {provide : HTTP_INTERCEPTORS, useClass : AuthInterceptor, multi : true},
    {provide : HTTP_INTERCEPTORS, useClass : ErrorInterceptor, multi : true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
