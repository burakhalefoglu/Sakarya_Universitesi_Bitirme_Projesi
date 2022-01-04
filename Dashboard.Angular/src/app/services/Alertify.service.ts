import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {



  constructor(private httpClient: HttpClient) {

    alertify.set('notifier', 'position', 'top-right');
  }

  success(message: string) {
    alertify.success(message);

  }

  error(message: string) {
    alertify.error(message);
  }

  info(message: string) {
    alertify.info(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  confirmDelete(url: string, values: any) {
    alertify.alert()
      .setting({
        'label': 'Agree',
        'message': 'This dialog is ',
        'onok': this.delete(url, values)
      }).show();
  }

  delete(url: string, values: any) {
    this.httpClient.request('delete', url, values)
  }




}
