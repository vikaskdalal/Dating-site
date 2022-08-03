import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { ModalComponent } from '../modal/modal.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef!: BsModalRef;

  constructor(private _modalService: BsModalService) { }

  confirm(title = 'Confirmation',
    message = 'Are you sure you want to do this?',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'): Observable<boolean> {
    const config = {
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText
      }
    }
    this.bsModelRef = this._modalService.show(ModalComponent, config);

    return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (observer: any) => {
      const subscription = this.bsModelRef.onHidden?.subscribe(() => {
        observer.next(this.bsModelRef.content.result);
        observer.complete();
      });

      return {
        unsubscribe() {
          subscription?.unsubscribe();
        }
      }
    }
  }
}
