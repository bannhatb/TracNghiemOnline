import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { UrlQuery } from "../Models/UrlQuery";
import { Response,ResponseDefault } from "../response.module";
import { BaseService } from "./base.service";

@Injectable({
  providedIn: 'root',
})

export class TestService extends BaseService implements CanActivate {
  constructor(public httpClient: HttpClient, public router: Router){
    super(httpClient);
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    throw new Error("Method not implemented.");
  }
  //info each user of test base testId (created)

  //get
  GetListTestBrief(urlQuery: UrlQuery){
    let url = `/api/Test/get-list-test-student?Page=${urlQuery.pageNumber}
    &PageSize=${urlQuery.pageSize}&Keyword=${urlQuery.keyword}`;
    return this.get(url);
  }
  GetUserTestStatus(testId: number){
    return this.get(`/api/Test/get-user-test-status?testId=${testId}`);
  }
  GetTestUser(testId: number, number : number){
    return this.get(`/api/Test/get-user-test?testId=${testId}&Page=${number}
    &PageSize=${number}&Keyword=${number}`);
  }
  GetOneQuestionUserTest(testId : number, page : number){
    return this.get(`/api/Test/get-one-question-user-test?testId=${testId}&page=${page}`);
  }
  GetAlo(){
    return this.get(`/api/Test/alo`);
  }
  GetUserTestPoint(testId : number){
    return this.get(`/api/Test/get-user-test-point?testId=${testId}`);
  }
  //post
  CreateTest(testForm: any) : Observable<Response<ResponseDefault>>{
    return this.post(`/api/Test/create-test`, testForm);
  }
  CreateTestUser(testId: number){
    return this.post(`/api/Test/create-test-user`, {testId:testId});
  }
  UserChooseAnswer(data : any){
    return this.post(`/api/Test/user-choose-answer`, data);
  }
  CacularPoint(testId : number){
    return this.post(`/api/Test/caculate-point`, {testId:testId});
  }
  AnalysisTestResult(testId : number, urlQuery: UrlQuery, classId: number) : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Test/get-test-result?testId=${testId}&classId=${classId}&Page=${urlQuery.pageNumber}&PageSize=${urlQuery.pageSize}&Keyword=${urlQuery.keyword}`);
  }
  GetListTestCreateBySelf() : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Test/get-list-test-create-by-self`);
  }
  CheckPass(testId : number ,password :string) : Observable<Response<ResponseDefault>>{
    let requestModel ={
      testId: testId,
      password: password
    }
    return this.post(`/api/Test/check-pass-test`, requestModel);
  }
  GetAllClass(): Observable<Response<ResponseDefault>>{
    return this.get(`/api/Test/get-all-class`);
  }
  GetListTestOfExam(examId : number) : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Exam/get-list-test-of-exam?examId=${examId}`);
  }
}
