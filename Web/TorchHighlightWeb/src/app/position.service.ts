import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class PositionService {

  
  // baseUri = "http://localhost:3141";
  // baseUri = "http://rhoffm2-de-le01:3141";
  baseUri = window.location;
  

  constructor(private httpClient: HttpClient) {
  }

  isSending = false;

  async sendY(y: number) {
    if (this.isSending){
      return;
    }
    this.isSending = true
    let url = this.baseUri + `?y=${y}`
    await this.httpClient.post(url, null).toPromise();
    this.isSending = false;
  }

  async sendRotate(rotate: number) {
    if (this.isSending){
      return;
    }
    this.isSending = true

    let url = this.baseUri + `?rotate=${rotate}`
    await this.httpClient.post(url, null).toPromise();

    this.isSending = false;
  }

  async sendDimm(dimmValue: boolean) {
    if (this.isSending){
      return;
    }
    this.isSending = true

    let url = this.baseUri + `?dimm=${dimmValue}`
    await this.httpClient.post(url, null).toPromise();

    this.isSending = false;
  }

}
