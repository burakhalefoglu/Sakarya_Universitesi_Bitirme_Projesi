import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {TokenModel} from '../../../model/token-model';
import {environment} from '../../../../../environments/environment';
import {MlInfoModel, MlInfoRequestModel} from '../../../model/ml-info-model';
import {Observable} from 'rxjs';
import {ClientsRequestModel} from '../../../model/client-model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  headers = new HttpHeaders();
  constructor(private httpClient: HttpClient) { }

  getApiInfo(typeKey: string): Observable<MlInfoRequestModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
   return this.httpClient.get<MlInfoRequestModel>(environment.getApiUrl + '/ApiInfoModels/getbytype?type=' + typeKey,
        { headers: this.headers })
  }

  getAllClients(): Observable<ClientsRequestModel> {
    this.headers = this.headers.append('Content-Type', 'application/json')
    return this.httpClient.get<ClientsRequestModel>(environment.getApiUrl + '/ClientModels/getall',
        { headers: this.headers })
  }
}

