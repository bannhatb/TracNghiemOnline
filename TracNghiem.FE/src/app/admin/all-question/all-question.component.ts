import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/services/account.service';
import { AdminService } from 'src/app/shared/services/admin.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { QuestionService } from 'src/app/shared/services/question.service';


@Component({
  selector: 'app-all-question',
  templateUrl: './all-question.component.html',
  styleUrls: ['./all-question.component.scss']
})
export class AllQuestionComponent implements OnInit {
  data: any
  listQuestion : any;
  listQuestionTeacher : any;
  Total: any;
  dem: number;
  dem1: number;
  dem2: number;
  roles: any;

  constructor(private adminService: AdminService,
    private questionService: QuestionService,
    private accountService: AccountService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.getAllQuestion();
    this.getAllQuestionOfUser();
    this.GetUserCurrent();
  }
  getAllQuestion(): void {
    this.adminService.GetAllQuestion().subscribe((res)=>{
      this.listQuestion = res;
      this.Total =this.listQuestion.result.total;
      console.log(this.listQuestion);
      console.log(this.Total);
    },(err)=>{
      console.log(err);

    })
  }

  deleteQuestion(id:any){
    this.questionService.DeleteQuestion(id).subscribe((res)=>
    {
      console.log('Da Xoa ' + id);
      window.confirm('Are you sure you want to delete');
      location.reload();
    },(err)=>{
      console.log(err.message);

    })
  }

  getAllQuestionOfUser(): void {
    this.questionService.GetListQuestionOfUser().subscribe((res)=>{
      this.listQuestionTeacher = res;
      this.Total =this.listQuestionTeacher.result.total;
      console.log(this.listQuestionTeacher);
      console.log(this.Total);
    },(err)=>{
      console.log(err);

    })
  }

  checkRole(listRoles : any){
    if(listRoles?.find((x: { roleId: number; })=>x.roleId==1) != null &&
    listRoles?.find((x: { roleId: number; })=>x.roleId==2) != null){
      return 1;
    }
    if(listRoles?.find((x: { roleId: number; })=>x.roleId==2) != null)
      return 2;
    else return 0;
  }

  GetUserCurrent(){
    this.accountService.GetUserDetailCurrent().subscribe((res)=>{
      this.data = res.result?.data
      console.log(this.data);
      this.roles = this.data.listRoles;
      console.log(this.roles)
      this.dem = (this.data.listTestDid).length;
      this.dem1 = (this.data.listTestCreate).length;
      this.dem2 = (this.data.listTestDid).length;
    },(err)=>{
      console.log(err.error.message);
    });
  }
}
