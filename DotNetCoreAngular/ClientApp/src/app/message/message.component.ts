import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  messages! : Message[] | null;
  pagination! : Pagination | null;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;

  constructor(private _messageService : MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this._messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(res => {
      this.messages = res.result;
      this.pagination = res.pagination;
    })
  }

  pageChanged(event : any){
    this.pageNumber = event.page;
    this.loadMessages()
  }

  loadNewMessages(container : string){
    this.container = container;
    this.loadMessages();
  }

}
