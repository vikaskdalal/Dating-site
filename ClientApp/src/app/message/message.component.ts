import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  messages!: Message[] | null;
  pagination!: Pagination | null;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;

  constructor(private _messageService: MessageService, private _titleService: Title) { }

  ngOnInit(): void {
    this.loadMessages();
    this._titleService.setTitle("Messages");
  }

  loadMessages() {
    this._messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(res => {
      this.messages = res.result;
      this.pagination = res.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadMessages()
  }

  deleteMessage(id: number) {
    this._messageService.deleteMessage(id).subscribe(() => {
      this.messages?.splice(this.messages.findIndex(m => m.id == id), 1);
    })

  }
  loadNewMessages(container: string) {
    this.container = container;
    this.loadMessages();
  }

}
