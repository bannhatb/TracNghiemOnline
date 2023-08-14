import { Component, OnInit } from "@angular/core";
import { TestService } from "src/app/shared/services/test.service";

@Component({
  selector: 'app-test-create',
  templateUrl: './test-create.component.html',
  styleUrls: ['./test-create.component.scss']
})
export class TestCreateComponent implements OnInit{
  data : any;
  constructor(private testService: TestService){}
  ngOnInit(): void {
    this.GetListTestCreate();
  }
  GetListTestCreate(){
    return this.testService.GetListTestCreateBySelf().subscribe((res)=>{
      this.data = res.result?.data;
      console.log(this.data);
    }, (err)=>{
      console.log(err.error.message);
    })
  }

}
