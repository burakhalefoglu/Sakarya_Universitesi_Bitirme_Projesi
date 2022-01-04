import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {TokenModel} from '../../../models/token-model';
import {environment} from '../../../../environments/environment';
import {ApiInfoModel, ApiInfoRequestModel} from '../../../models/api-info-model';
import {Observable} from 'rxjs';
import {ClientsRequestCountModel, ClientsRequestModel} from '../../../models/client-model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  headers = new HttpHeaders();
  constructor(private httpClient: HttpClient) { }

  getApiInfo(typeKey: string): Observable<ApiInfoRequestModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
   return this.httpClient.get<ApiInfoRequestModel>(environment.getApiUrl + '/ApiInfoModels/getbytype?type=' + typeKey,
        { headers: this.headers })
  }

  getLastClientsByCount(count: number): Observable<ClientsRequestModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
    return this.httpClient.get<ClientsRequestModel>(environment.getApiUrl + '/ClientModels/getlastclientsbycount?count=' + count,
        { headers: this.headers })
  }

  getPositiveSentimentRate(): Observable<ClientsRequestCountModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
    return this.httpClient.get<ClientsRequestCountModel>(environment.getApiUrl + '/ClientModels/getpositivesentimentrate',
        { headers: this.headers })
  }

 getTotalClientCount(): Observable<ClientsRequestCountModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
    return this.httpClient.get<ClientsRequestCountModel>(environment.getApiUrl + '/ClientModels/gettotalclientcount',
        { headers: this.headers })
  }

 getPositiveSentimentRateByDate(startDate: number, finishDate: number ): Observable<ClientsRequestCountModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
    return this.httpClient.get<ClientsRequestCountModel>(environment.getApiUrl + '/ClientModels/getpositivesentimentratebydate' +
       '?startDate=' +
        startDate +
        '&finishDate=' +
        finishDate,
        { headers: this.headers })
  }

}

