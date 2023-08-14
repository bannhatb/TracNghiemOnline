import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NotificationService } from "src/app/shared/services/notification.service";
import { TestService } from "src/app/shared/services/test.service";

@Component({
  selector: 'app-user-point',
  templateUrl: './user-point.component.html',
  styleUrls: ['./user-point.component.scss']
})
export class UserPointComponent implements OnInit {
  pointData: any;
  testId : number;
  constructor(
    private router: Router,
    private testService: TestService,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService
  ){}
  async ngOnInit(): Promise<void> {
    this.GetPoint();
    await new Promise(f => setTimeout(f, 1000));
    this.GetPoint();
  }
  GetPoint(){
    this.activeRoute.params.subscribe((testId)=>{
      this.testId = testId.testId;
      console.log(this.testId);
    })
    this.testService.GetUserTestPoint(this.testId).subscribe((res)=>{
      this.pointData = res;
      console.log(this.pointData);

    },(err) =>{

      console.log(err.error.message);
      this.notificationService.info('Bạn chưa làm đề thi này');
    }, ()=>{
    });
  }
  close(){
    window.close();
  }

}
