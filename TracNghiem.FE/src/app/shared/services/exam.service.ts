import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { UrlQuery } from "../Models/UrlQuery";
import { ResponseDefault, Response, Pagination } from "../response.module";
import { BaseService } from "./base.service";

@Injectable({
  providedIn: 'root'
})
export class ExamService extends BaseService implements CanActivate{
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    throw new Error("Method not implemented.");
  }
  //Get
  CheckAuthenTeacher(){
    return this.get(`/api/Exam/check-authen-techer`);
  }
  GetListExamUser(urlQuery: UrlQuery) : Observable<Response<Pagination<any>>>{
    return this.get(`/api/Exam/get-list-exam-of-user?page=${urlQuery.pageNumber}&pageSize=${urlQuery.pageSize}&keyWord=${urlQuery.keyword}`);
  }
  GetListQuestionOfExam(examId: number,urlQuery: UrlQuery) : Observable<Response<Pagination<any>>>{
    return this.get(`/api/Exam/get-list-question-of-exam?examId=${examId}&page=${urlQuery.pageNumber}&pageSize=${urlQuery.pageSize}&keyWord=${urlQuery.keyword}`);
  }
  GetCategory(){
    return this.get('/api/Catelog/get-all-category');
  }
  DeleteQuestionExam(examId : number, questionId: number) : Observable<Response<ResponseDefault>>{
    return this.delete
    (`/api/Question/delete-question-exam?examId=${examId}&questionId=${questionId}`);
  }
  CreateExam(examDto: any){
    return this.post(`/api/Exam/add-exam`, examDto);
  }
  GenQuestionAuto(requestModel : any){
    return this.post(`/api/Exam/word-to-exam`, requestModel);
  }
  UpFileWord(file: File): Observable<any>{
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.postFile<any>(`/api/File/upload-file`, formData,  this.getHeaders());
  }
  DeleteExam(id : number){
    return this.post(`/api/Exam/delete-exam`, id);
  }
  getExambycatalog(cateId: number) : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Exam/get-list-exam-by-category?cateId=${cateId}`)
  }
  GetAllcategory(): Observable<Response<ResponseDefault>>{
    return this.get(`/api/Catelog/get-all-category`)
  }
}
