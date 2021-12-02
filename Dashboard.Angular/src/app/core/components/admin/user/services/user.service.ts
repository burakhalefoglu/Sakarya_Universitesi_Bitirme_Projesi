import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { environment } from '../../../../../../environments/environment'
import { LookUp } from 'app/core/models/lookUp';
import { PasswordDto } from '../models/passwordDto';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getUserList(): Observable<User[]> {

    return this.httpClient.get<User[]>(environment.getApiUrl + "/users/getall");

  }

  getUserById(id: number): Observable<User> {

    return this.httpClient.get<User>(environment.getApiUrl + "/users/getbyid?userId=" + id);
  }


  addUser(user: User): Observable<any> {

    var result = this.httpClient.post(environment.getApiUrl + "/users/", user, { responseType: 'text' });
    return result;
  }

  updateUser(user:User):Observable<any> {

    var result = this.httpClient.put(environment.getApiUrl + "/users/", user, { responseType: 'text' });
    return result;
  }

  deleteUser(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + "/users/", { body: {userId:id} });
  }

  getUserGroupPermissions(userId:number):Observable<LookUp[]>{
    return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/UserGroups/getusergroupbyuserid?id=" + userId);
  }

  getUserClaims(userId:number){
     return this.httpClient.get<LookUp[]>(environment.getApiUrl + "/UserClaims/getoperationclaimbyuserid?id=" + userId);
  }

  saveUserClaims(userId:number,claims:number[] ):Observable<any> {

    var result = this.httpClient.put(environment.getApiUrl + "/UserClaims/", {UserId:userId, ClaimId:claims }, { responseType: 'text' });
    return result;

  }

  saveUserGroupPermissions(userId:number, groups:number[]):Observable<any> {
    var result = this.httpClient.put(environment.getApiUrl + "/UserGroups/", {UserId:userId, GroupId:groups }, { responseType: 'text' });
    return result;

  }

  saveUserPassword(command:PasswordDto):Observable<any>{
    var result = this.httpClient.put(environment.getApiUrl + "/Auth/changeuserpassword", command, { responseType: 'text' });
    return result;
  }

}
