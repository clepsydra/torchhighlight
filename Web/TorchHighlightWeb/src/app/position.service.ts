import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class PositionService {

  
  // baseUri = "http://localhost:3141";
  // baseUri = "http://rhoffm2-de-le01:3141";
  baseUri = window.location;
  

  constructor(private httpClient: HttpClient) {
  }

  isSendingY = false;

  async sendY(y: number) {
    if (this.isSendingY){
      return;
    }
    this.isSendingY = true
    let url = this.baseUri + `?y=${y}`
    try
    {
      await this.httpClient.post(url, null).toPromise();
    }
    catch {}
    this.isSendingY = false;
  }

  isSendingRotate = false;

  async sendRotate(rotate: number) {
    if (this.isSendingRotate){
      return;
    }
    this.isSendingRotate = true

    let url = this.baseUri + `?rotate=${rotate}`
    try
    {
      await this.httpClient.post(url, null).toPromise();
    }
    catch {}

    this.isSendingRotate = false;
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
