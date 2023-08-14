import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { ErrorCode } from 'src/app/shared/response.module';
import * as signalR from '@microsoft/signalr';
import { TestService } from 'src/app/shared/services/test.service';
import { environment } from 'src/environments/environment';
import { SignalRService } from 'src/app/shared/services/signalR.service';

@Component({
  selector: 'app-user-test',
  templateUrl: './do-test.component.html',
  styleUrls: ['./do-test.component.scss']
})
export class DoTestComponent implements OnInit {
  testId : number;
  result: any;
  data : any;
  choose = new Array<number>();
  urlQuery : UrlQuery = new UrlQuery(1,1,'');
  page: number =1;
  isChoosed = new Array<number>();
  total : any =0;
  TotalPage : number;
  TestUserInfo : any;
  TestTime : number =0;

  constructor(private testService: TestService,
    private activeRoute: ActivatedRoute,
    private router: Router) {

     }

  ngOnInit() {
    this.GetTime();
    this.getQuestionTestUser();
  }

  GetTime(){
    this.activeRoute.params.subscribe((testId) =>{
      this.testId= testId.testId;
    })
      this.testService.GetUserTestStatus(this.testId).subscribe((res)=>{

      this.TestUserInfo = res;
      this.TestTime = this.TestUserInfo.result.data.time;
      console.log(this.TestTime);
    }, err=>{
      console.log(err.error.message)
    }, ()=>{
      //console.log(this.result);
    })
  }

  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.getQuestionTestUser();
  }
  getQuestionTestUser(){
    if(this.choose.length > 0){
      this.SubmitAnswer();
      this.choose = new Array<number>();
    }
    this.activeRoute.params.subscribe((testId) =>{
      this.testId= testId.testId;
    })
    this.testService.GetOneQuestionUserTest(this.testId, this.urlQuery.pageNumber).subscribe((res)=>{
      this.result = res;
      this.total = res;
      this.TotalPage = Math.ceil(this.total.result?.data.total/this.urlQuery.pageSize);
      if(this.result.result.data.questions.userChoose.length > 0){
        this.isChoosed = new Array<number>();
        for(let index in this.result.result.data.questions.userChoose){
          this.isChoosed.push(this.result.result.data.questions.userChoose[index].choose);
        }
        this.choose = this.isChoosed;
      }
    }, err=>{
      console.log(err.error.message);
    }, ()=>{
      //console.log(this.result);

    });
  }
  //hàm kiểm tra các câu trả lời user đã chọn để render ui
    usChoosed(id: number) {
    return this.isChoosed.some(function(el) {
      return el === id;
    });
  }
  SubmitTest(){
    this.testService.CacularPoint(this.testId).subscribe((res)=>{
      //console.log(res);
    })
    this.router.navigateByUrl(`/test/user-point/${this.testId}`);
  }
  chooseOne(event: any){
    if(event.target.checked){
      this.choose.splice(0,1);
      this.choose.push(event.target.defaultValue);
    }
    else{
      let index = this.choose.indexOf(event.target.defaultValue);
      this.choose.splice(index,1);
    }
    console.log(this.choose);
  }
  // khi thay đổi giá trị input checkbox thực thi hàm này để lấy data
  chooseChange(event: any){
    if(event.target.checked){
      this.choose.push(event.target.defaultValue);
    }
    else{
      let index = this.choose.indexOf(event.target.defaultValue);
      this.choose.splice(index,1);
    }
    console.log(this.choose);
  }
  SubmitAnswer(){
    let data = {
      testQuestionId : this.result.result.data.questions.id,
      answerIds : this.choose
    }
    this.testService.UserChooseAnswer(data).subscribe((res)=>{
    }, err => console.log(err.error.message));
  }
}
