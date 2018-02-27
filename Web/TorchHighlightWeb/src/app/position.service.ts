import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class PositionService {

  async sendDimm(dimmValue: boolean) {
    let url = this.baseUri + `?dimm=${dimmValue}`
    await this.httpClient.post(url, null).toPromise();
  }
  // baseUri = "http://localhost:3141";
  // baseUri = "http://rhoffm2-de-le01:3141";
  baseUri = window.location;
  

  constructor(private httpClient: HttpClient) {
  }

  async sendY(y: number) {
    let url = this.baseUri + `?y=${y}`
    await this.httpClient.post(url, null).toPromise();
  }

  async sendRotate(rotate: number) {
    let url = this.baseUri + `?rotate=${rotate}`
    await this.httpClient.post(url, null).toPromise();
  }

}
