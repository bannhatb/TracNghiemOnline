import { Component, OnInit } from '@angular/core';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { ErrorCode } from 'src/app/shared/response.module';
import { AdminService } from 'src/app/shared/services/admin.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  urlQuery = new UrlQuery();
  classIds  = new Array<number>();
  data : any;
  total : any =0;
  TotalPage : number;
  constructor(private adminService: AdminService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.classIds.push(0);
    this.GetUser("");
  }
  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.GetUser(this.urlQuery.keyword);
  }
  GetUser(text : string){
    this.urlQuery.keyword = text;
    this.adminService.GetAllUser(this.urlQuery, this.classIds).subscribe((res)=>{
      this.data = res.result;
      console.log(this.data);
      this.total = res.result?.total;
      this.TotalPage = Math.ceil(this.total/this.urlQuery.pageSize);
    },(err)=>{
      console.log(err.error.message)
    });
  }
  changeStatus(userId: number){
    this.adminService.ChangeStatus(userId).subscribe((res)=>{
      console.log(res);
      location.reload();
      this.notificationService.success("Thành công");
    },(err)=>{
      console.log(err.error.message);
      if(err.error.message == ErrorCode.BlockAdmin){
        this.notificationService.warn("Không thể khóa tài khoản admin!");
      }
    });
  }
  checkRole(user : any){
    if(user.listRoles.find((x: { roleId: number; })=>x.roleId==2) != null &&
    user.listRoles.find((x: { roleId: number; })=>x.roleId==1) != null){
      return 1;
    }
    if(user.listRoles.find((x: { roleId: number; })=>x.roleId==1) != null)
      return 2;
    if(user.listRoles.find((x: { roleId: number; })=>x.roleId==2) != null)
      return 3;
    else return 0;
  }
  AuthenTeacher(userId : number){
    return this.adminService.AuthenTeacher(userId).subscribe((res)=>{
      console.log(res);
      this.notificationService.success("Thành công");
    }, (err)=>{
      console.log(err.error.message);
      alert("đã có lỗi xảy ra");
    }, ()=>{
      location.reload();
    })
  }
  AuthenAdmin(userId : number){
    return this.adminService.AuthenAdmin(userId).subscribe((res)=>{
      console.log(res);
      this.notificationService.success("Thành công");
    }, (err)=>{
      console.log(err.error.message);
      alert("đã có lỗi xảy ra");
    }, ()=>{
      location.reload();
    })
  }

}
