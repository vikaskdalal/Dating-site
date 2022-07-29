import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { debounceTime, fromEvent } from 'rxjs';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit, AfterViewInit{
  @Input() messages!: Message[];
  @Input() username! : string;
  messageContent!: string;
  @ViewChild('messageForm') messageForm! : NgForm;
  sentTypingEvent : boolean = true;
  @ViewChild('chatBox') chatBox!: ElementRef;
  isRecipientTyping : boolean = false;
  constructor(public messageService : MessageService) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {     
     this.sendEventWhenUserStopsTyping();
     this.messageService.recipientIsTypingSource$.subscribe(res => {
      this.isRecipientTyping = res.filter(f => f.username == this.username).length != 0;
     })
  }

  sendEventWhenUserStopsTyping(){
    fromEvent(this.chatBox.nativeElement, 'input')
    .pipe(debounceTime(1000))
    .subscribe(data => {
      this.messageService.sendUserHasStoppedTypingEvent(this.username).then(() => {
        this.sentTypingEvent = true;
      });
      
    });  
  }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
        this.messageForm.reset();
    });
  }

  onKeyUpEvent(event: any){
    if(!this.sentTypingEvent)
      return;

    this.messageService.sendUserIsTypingEvent(this.username).then(()=> {
      this.sentTypingEvent = false;
    });
  }

}
