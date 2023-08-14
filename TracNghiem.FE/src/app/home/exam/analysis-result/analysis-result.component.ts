import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { UrlQuery } from "src/app/shared/Models/UrlQuery";
import { TestService } from "src/app/shared/services/test.service";

@Component({
  selector: 'app-analysis-result',
  templateUrl: './analysis-result.component.html',
  styleUrls: ['./analysis-result.component.scss']
})
export class AnalysisResultComponent implements OnInit{
  urlQuery = new UrlQuery();
  testId : number = 0;
  classId : any = -1;
  data : any;
  classData : any;
  Total : any =0;
  TotalPage : number;
  constructor(private testService: TestService, private activeRoute: ActivatedRoute){}
  ngOnInit(): void {
    this.GetAllClass();
    this.GetTestAnalysisResult(this.urlQuery.keyword);
  }
  ChangeClass(classId : string){
    this.GetTestAnalysisResult(this.urlQuery.keyword);
  }
  GetAllClass(){
    this.testService.GetAllClass().subscribe((res)=>{
      this.classData = res.result?.data;
      console.log(this.classData);
    }, (err)=>{
      console.log(err.error.message);
    })
  }
  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.GetTestAnalysisResult(this.urlQuery.keyword);
  }
  GetTestAnalysisResult(text : string){
    this.activeRoute.params.subscribe((id)=>{
      this.testId= id.id;
    })
    this.urlQuery.keyword = text;
    this.testService.AnalysisTestResult(this.testId, this.urlQuery, this.classId).subscribe((res)=>{
      this.data = res.result?.data;
      console.log(this.data);
      this.Total = res.result?.data.total;
      this.TotalPage = Math.ceil(this.Total/this.urlQuery.pageSize);
    }, (err)=>{
      console.log(err.error.message);
    });
  }




}
