import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { QuestionFullModel } from "../entities/question.module";
import { Pagination, Response, ResponseDefault } from "../response.module";
import { BaseService } from "./base.service";

@Injectable({
  providedIn: 'root',
})

export class QuestionService extends BaseService implements CanActivate {
  constructor(public httpClient: HttpClient, public router: Router){
    super(httpClient);
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean  {
    throw new Error("Method not implemented.");
  }
  GetQuestionDetail(questionId : string) : Observable<Response<QuestionFullModel>>{
    return this.get(`/api/Question/get-question-detail?questionId=${questionId}`);
  }
  AddNewQuestion(questionForm: any) : Observable<Response<ResponseDefault>>{
    return this.post(`/api/Question/add-question`, questionForm);
  }
  UpdateQuestion(questionForm: any): Observable<Response<ResponseDefault>>{
    return this.post(`/api/Question/update-question`, questionForm);
  }
  AddQuestionToExam(queExamForm: any) : Observable<Response<ResponseDefault>>{
    return this.post(`/api/Question/add-question-to-exam`, queExamForm);
  }
  DeleteQuestion(id: number): Observable<Response<ResponseDefault>>{
    return this.delete(`/api/Question/delete-question/${id}`);
  }
  GetListQuestionOfUser() {
    return this.get(`/api/Question/get-list-question-of-user`);
  }
  GetAllLevel(): Observable<Response<ResponseDefault>>{
    return this.get('/api/Home/get-all-level');
  }

}
