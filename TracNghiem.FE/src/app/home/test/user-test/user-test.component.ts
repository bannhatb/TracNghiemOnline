import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { ErrorCode } from 'src/app/shared/response.module';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SignalRService } from 'src/app/shared/services/signalR.service';
import { TestService } from 'src/app/shared/services/test.service';

@Component({
  selector: 'app-user-test',
  templateUrl: './user-test.component.html',
  styleUrls: ['./user-test.component.scss']
})
export class UserTestComponent implements OnInit {
  testId : number;
  status : any;
  result: any;
  urlQuery : UrlQuery = new UrlQuery(1,1,'');
  passWord : string;
  constructor(private testService: TestService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private signalRService: SignalRService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe((id) => {
      this.testId = id.id;
    });
    this.createTestUser();
  }
  CheckPass(){
    this.activeRoute.params.subscribe((id) => {
      this.testId= id.id;
    })
    this.testService.CheckPass(this.testId, this.passWord).subscribe((res)=>{
      if(res.message == ErrorCode.WrongPasss){
        this.notificationService.error("Sai mật khẩu!!");
      }
      if(res.message == ErrorCode.Success){
        this.createTestUser();
      }
    })
  }

  createTestUser() {
    this.testService.CreateTestUser(this.testId).subscribe((res) =>{
      console.log(res);
      this.status = res;
    }, err=>{
      console.log(err.error.message)
    }, ()=>{
      this.testService.GetUserTestStatus(this.testId).subscribe((res)=>{
        this.result = res;
      }, err=>{
        console.log(err.error.message)
      }, ()=>{
        console.log(this.result);
      })
    });
  }
  doTest() {
    this.signalRService.startConect();
    console.log(this.signalRService.hubConnection);
    let time = this.result.result.data.time;
    this.signalRService.TimeRun(time,this.testId);
    this.router.navigateByUrl(`/test/do-test/${this.testId}`);
  }
}
