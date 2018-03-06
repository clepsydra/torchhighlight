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
    try
    {
      await this.httpClient.post(url, null).toPromise();
    }
    catch {}
    this.isSending = false;
  }

  async sendRotate(rotate: number) {
    if (this.isSending){
      return;
    }
    this.isSending = true

    let url = this.baseUri + `?rotate=${rotate}`
    try
    {
      await this.httpClient.post(url, null).toPromise();
    }
    catch {}

    this.isSending = false;
  }

  async sendDimm(dimmValue: boolean) {
    

    let url = this.baseUri + `?dimm=${dimmValue}`
    try
    {
      await this.httpClient.post(url, null).toPromise();
    }
    catch {}

  }

}
