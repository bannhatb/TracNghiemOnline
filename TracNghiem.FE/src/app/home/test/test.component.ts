import { Component, OnInit } from '@angular/core';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { TestService } from 'src/app/shared/services/test.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
  test: any;
  total : any =0;
  TotalPage : number;
  constructor(private testServices : TestService) { }
  urlQuery:UrlQuery = new UrlQuery();
  ngOnInit(): void {
    this.getListTest(this.urlQuery.keyword);
  }
  getListTest(text: string){
    var result : any;
    this.urlQuery.keyword = text;
    this.testServices.GetListTestBrief(this.urlQuery).subscribe((res)=>{
      result= res;
      this.total = res;
      this.TotalPage = Math.ceil(this.total.result?.total/this.urlQuery.pageSize);
    },(err) =>{
      console.log(err.error.message);
    },
    ()=>{
      console.log(result);
      this.test = result.result.items;
      this.total = result.result.total;
    });
  }
  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.getListTest(this.urlQuery.keyword);
  }

}
