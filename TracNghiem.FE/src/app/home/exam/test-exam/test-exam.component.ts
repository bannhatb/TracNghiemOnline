import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestService } from 'src/app/shared/services/test.service';

@Component({
  selector: 'app-test-exam',
  templateUrl: './test-exam.component.html',
  styleUrls: ['./test-exam.component.scss']
})
export class TestExamComponent implements OnInit {

  data : any;
  examId : number;
  constructor(private testService: TestService, private activeRoute: ActivatedRoute){}
  ngOnInit(): void {
    this.activeRoute.params.subscribe((id)=>{
      this.examId = id.id;
    })
    this.GetListTestExam(this.examId);
  }
  GetListTestExam(examId : number){
    return this.testService.GetListTestOfExam(examId).subscribe((res)=>{
      this.data = res.result?.data;
      console.log(this.data);
    }, (err)=>{
      console.log(err.error.message);
    })
  }

}
