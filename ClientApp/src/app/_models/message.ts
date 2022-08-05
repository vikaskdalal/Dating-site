export interface Message {
  id: number;
  senderUsername: string;
  senderName: string;
  senderPhotoUrl: string;
  recipientUsername: string;
  recipientName: string;
  recipientPhotoUrl: string;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}