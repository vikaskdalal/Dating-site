import { AfterContentInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit, AfterContentInit{
  @Input() messages!: Message[];
  @Input() username! : string;
  messageContent!: string;
  @ViewChild('messageForm') messageForm! : NgForm;
  @ViewChild('chatContainer') private myScrollContainer! : ElementRef;

  constructor(public messageService : MessageService) { }

  ngOnInit(): void {
    this.scrollToBottom();
  }

  ngAfterContentInit() {        
    this.scrollToBottom();        
  }

scrollToBottom(): void {
    try {
        //this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch(err) { }                 
  }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
        this.messageForm.reset();
    });
  }

}
