import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-user-chat',
  templateUrl: './user-chat.component.html',
  styleUrls: ['./user-chat.component.css']
})
export class UserChatComponent implements OnInit {
  @Input() messages!: Message[];
  @Input() username! : string;
  messageContent!: string;
  @ViewChild('messageForm') messageForm! : NgForm;

  constructor(public messageService : MessageService) { }

  ngOnInit(): void {
  }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
        this.messageForm.reset();
    });
  }

}
