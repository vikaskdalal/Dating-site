import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'chatDate'
})
export class ChatDatePipe implements PipeTransform {

  constructor(private _datePipe: DatePipe){}
  transform(value: any, ...args: any[]): any {
    if(!value)
      return value;
    
    let dateFormat = 'd/M/yy';
    if(args.length)
      dateFormat = args[0];

    let currentDate = this._datePipe.transform(new Date(), dateFormat);
    let formatValue = this._datePipe.transform(value, dateFormat);

    if(currentDate == formatValue)
      return 'Today';
    else
      return formatValue;
  }

}
