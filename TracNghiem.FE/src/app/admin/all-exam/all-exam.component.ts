import { Component, OnInit } from '@angular/core';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { AccountService } from 'src/app/shared/services/account.service';
import { AdminService } from 'src/app/shared/services/admin.service';
import { ExamService } from 'src/app/shared/services/exam.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-all-exam',
  templateUrl: './all-exam.component.html',
  styleUrls: ['./all-exam.component.scss']
})
export class AllExamComponent implements OnInit {
  urlQuery = new UrlQuery();
  data: any;
  ListExamAdmin : any;
  ListExamTeacher : any;
  Total : any =0;
  TotalPage: number;
  dem: number;
  dem1: number;
  dem2: number;
  roles: any;

  constructor(private adminService: AdminService,
    private examService: ExamService,
    private accountService: AccountService,
    private notificationService: NotificationService) { }

    ngOnInit(): void {
      this.GetAllExamAdmin(this.urlQuery.keyword);
      this.GetAllExamTeacher(this.urlQuery.keyword);
      this.GetUserCurrent();
    }
    ChangePageHandler(page : number){
      this.urlQuery.pageNumber = page;
      this.GetAllExamAdmin(this.urlQuery.keyword);
    }
    GetAllExamAdmin(text : string){
      this.urlQuery.keyword = text;
      this.adminService.GetAllExam(this.urlQuery).subscribe((res)=>{
        this.ListExamAdmin = res;
        console.log(this.ListExamAdmin);
        this.Total = this.ListExamAdmin.result?.total;
        this.TotalPage = Math.ceil(this.Total/this.urlQuery.pageSize);
      }, err => {
        console.log(err);
      }, ()=>{

      })
    }

    ChangePageHandlerTeacher(page : number){
      this.urlQuery.pageNumber = page;
      this.GetAllExamTeacher(this.urlQuery.keyword);
    }
    GetAllExamTeacher(text : string){
      this.urlQuery.keyword = text;
      this.examService.GetListExamUser(this.urlQuery).subscribe((res)=>{
        this.ListExamTeacher = res;
        console.log(this.ListExamTeacher);
        this.Total = this.ListExamTeacher.result?.total;
        this.TotalPage = Math.ceil(this.Total/this.urlQuery.pageSize);
      }, err => {
        console.log(err);
      }, ()=>{

      })
    }
  deleteExam(id : number){
    this.adminService.DeleteExamAdmin(id).subscribe((res)=>{
      console.log(res)
      this.notificationService.success("Đã xóa bài thi");
      location.reload();
    }, (err)=>{
      console.log(err);
    });
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
