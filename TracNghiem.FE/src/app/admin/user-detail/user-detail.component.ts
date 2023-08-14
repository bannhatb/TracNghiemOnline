import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { AdminService } from 'src/app/shared/services/admin.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {
  userId : number;
  data: any;
  dem: number;
  dem1: number;
  dem2: number;
  urlQuery = new UrlQuery();
  constructor(private activeRoute: ActivatedRoute,
    private adminService: AdminService) { }

  ngOnInit(): void {
    this.GetUserDetail();
  }
  GetUserDetail(){
    this.activeRoute.params.subscribe((id)=>{
      this.userId= id.id;
    })
    this.adminService.GetUserDetail(this.userId, this.urlQuery).subscribe((res)=>{
      this.data = res.result?.data
      console.log(this.data);
      this.dem = (this.data.listTestDid).length;
      this.dem1 = (this.data.listTestCreate).length;
      this.dem2 = (this.data.listTestDid).length;
    },(err)=>{
      console.log(err.error.message);
    });
  }


}
