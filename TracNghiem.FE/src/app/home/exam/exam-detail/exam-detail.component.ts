import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { ExamService } from 'src/app/shared/services/exam.service';

@Component({
  selector: 'app-exam-detail',
  templateUrl: './exam-detail.component.html',
  styleUrls: ['./exam-detail.component.scss']
})
export class ExamDetailComponent implements OnInit {
  examId: number;
  ListQuestion: any;
  total : any =0;
  urlQuery = new UrlQuery();
  TotalPage : number;
  constructor(private examService: ExamService, private activeRoute : ActivatedRoute) {

   }

  ngOnInit(): void {
    this.GetListQuestionOfExam(this.urlQuery.keyword);
  }
  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.GetListQuestionOfExam(this.urlQuery.keyword);
  }
  GetListQuestionOfExam(text: string){
    this.activeRoute.params.subscribe((id)=>{
      this.examId = id.id;
    })
    this.urlQuery.keyword = text;
    this.examService.GetListQuestionOfExam(this.examId, this.urlQuery).subscribe((res)=>{
      this.ListQuestion = res.result?.items;
      if(this.ListQuestion != undefined){
        this.total = res.result?.total;
        this.TotalPage = Math.ceil(this.total/this.urlQuery.pageSize);
      }
      console.log(this.ListQuestion);
      console.log(res);
      
    }, (err)=>{
      console.log(err.error.message);
    })
  }
  ChangeRight(event : any){

  }

}
