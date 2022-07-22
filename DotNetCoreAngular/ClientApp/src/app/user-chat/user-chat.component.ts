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

  constructor(private _messageService : MessageService) { }

  ngOnInit(): void {
  }

  sendMessage() {
    this._messageService.sendMessage(this.username, this.messageContent).subscribe(msg => {
        this.messages.push(msg);
        this.messageForm.reset();
    });
  }

}
