import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { UrlQuery } from "../Models/UrlQuery";
import { Response, Pagination, ResponseDefault } from "../response.module";
import { BaseService } from "./base.service";

@Injectable({
  providedIn : 'root'
})
export class AdminService extends BaseService implements CanActivate{
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    throw new Error("Method not implemented.");
  }
  GetAllUser(urlQuery : UrlQuery, classIds : Array<number>) : Observable<Response<Pagination<any>>>{
    let url = `/api/Admin/get-all-user?page=${urlQuery.pageNumber}
    &pageSize=${urlQuery.pageSize}&keyWord=${urlQuery.keyword}`;
    for(let i =0; i< classIds.length; i++){
      url =url.concat(`&classIds=${classIds[i]}`);
    }
    return this.get(url);
  }
  GetUserDetail(userId: number, urlQuery: UrlQuery) : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Admin/get-user-detail?userId=${userId}&page=${urlQuery.pageNumber}
    &pageSize=${urlQuery.pageSize}&keyWord=${urlQuery.keyword}`);
  }
  ChangeStatus(userId : number): Observable<Response<ResponseDefault>>{
    return this.post(`/api/Admin/block-user`, {userId:userId});
  }
  AuthenTeacher(userId : number){
    return this.post(`/api/Admin/authen-teacher`, {userId:userId})
  }
  AuthenAdmin(userId : number){
    return this.post(`/api/Admin/authen-admin`, {userId:userId})
  }
  GetAllExam(urlQuery: UrlQuery) : Observable<Response<Pagination<any>>>{
    return this.get(`/api/Admin/get-all-exam?page=${urlQuery.pageNumber}&pageSize=${urlQuery.pageSize}&keyWord=${urlQuery.keyword}`);
  }
  DeleteExamAdmin(id : number){
    return this.post(`/api/Admin/delete-exam-admin`, id);
  }
  GetAllQuestion(){
    return this.get(`/api/Admin/get-all-question`);
  }
}
