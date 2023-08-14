import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnswerModel } from 'src/app/shared/entities/answer.module';
import { QuestionService } from 'src/app/shared/services/question.service';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.scss']
})
export class QuestionDetailComponent implements OnInit {
  id: string;
  questionId: any;
  question: any;
  ListQuestionAnswer: AnswerModel[];
  total : any = 1;
  // urlQuery = new UrlQuery();
  // TotalPage : number;
  // activeRoute: any;
  constructor(
    private questionService: QuestionService,
    private activeRoute : ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.GetQuestionDetail();
    this.ListQuestionAnswer = [];
  }

  GetQuestionDetail() {
    this.activeRoute.params.subscribe((id)=>{
        this.questionId = id.id;
      })
    this.questionService.GetQuestionDetail(this.questionId).subscribe((res)=>{
      this.question = res;
      console.log(res)
    }, (err)=>{
      console.log(err.message);
    })
  }
}
