import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { QuestionFullModel } from 'src/app/shared/entities/question.module';
import { QuestionService } from 'src/app/shared/services/question.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.scss']
})
export class QuestionComponent implements OnInit {
  id: string;
  ListQuestion: any;
  ListQuestionAnswer:any[];
  total : any = 1;
  constructor(
    private questionService: QuestionService,
    private activeRoute : ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.ListQuestionAnswer = [];
    this.GetListQuestionOfUser();
  }

  GetListQuestionOfUser(){
    this.questionService.GetListQuestionOfUser().subscribe((res)=>{
      this.ListQuestion = res;
      this.ListQuestion.result.data.forEach((val:any)=>{
        this.questionService.GetQuestionDetail(String(val.id)).subscribe((res)=>{
          this.ListQuestionAnswer.push(res)

        })
      })
      console.log(this.ListQuestionAnswer);

      // console.log(this.ListQuestion);
    }, (err)=>{
      console.log(err.message);
    })
  }
  deleteQuestion(id:any){
    this.questionService.DeleteQuestion(id).subscribe((res)=>
    {
      console.log('Da Xoa ' + id);
      window.confirm('Are you sure you want to delete');
      this.GetListQuestionOfUser();

    },(err)=>{
      console.log(err.message);

    })
  }

}


