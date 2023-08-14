import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UrlQuery } from 'src/app/shared/Models/UrlQuery';
import { AdminService } from 'src/app/shared/services/admin.service';
import { ExamService } from 'src/app/shared/services/exam.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-exam',
  templateUrl: './exam.component.html',
  styleUrls: ['./exam.component.scss']
})
export class ExamComponent implements OnInit {
  urlQuery = new UrlQuery();
  ListExam : any;
  Total : any =0;
  TotalPage : number;
  listCategory: any;
  constructor(private examService : ExamService, private notificationService: NotificationService,
    private adminService: AdminService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.router.navigateByUrl('/page').then(() => {
      window.location.reload();
    });
    // this.GetListExamCreateCurrentUser(this.urlQuery.keyword);
    this.GetAllExamShow(this.urlQuery.keyword);
  }
  ChangePageHandler(page : number){
    this.urlQuery.pageNumber = page;
    this.GetListExamCreateCurrentUser(this.urlQuery.keyword);
  }
  GetListExamCreateCurrentUser(text : string){
    this.urlQuery.keyword = text;
    this.examService.GetListExamUser(this.urlQuery).subscribe((res)=>{
      this.ListExam = res.result?.items;
      // console.log(this.urlQuery.keyword);
      this.Total = res.result?.total;
      this.TotalPage = Math.ceil(this.Total/this.urlQuery.pageSize);
    }, err => {
      console.log(err);
    }, ()=>{

    })
  }
  deleteExam(id : number){
    this.examService.DeleteExam(id).subscribe((res)=>{
      console.log(res)
      this.notificationService.success("Đã xóa bài thi");
      location.reload();
    }, (err)=>{
      console.log(err);
    });
  }
  async newCategory(event : any){
    let value = event.target.value;
    if(value=='0'){
      const exams = await this.adminService.GetAllExam(this.urlQuery).toPromise();
      this.ListExam = exams.result;
    }
    else{
      const exam = await this.examService.getExambycatalog(value).toPromise();
      this.ListExam = exam.result;
      this.Total = this.ListExam.total;
      console.log(exam.result);
    }
  }
  async GetAllExamShow(text: string) {
    this.urlQuery.keyword = text;
    const res = await this.examService.GetAllcategory().toPromise();
    this.listCategory = res
    const exams = await this.adminService.GetAllExam(this.urlQuery).toPromise();
    this.ListExam = exams.result;
    this.TotalPage = Math.ceil(this.Total/this.urlQuery.pageSize);
    this.Total = 1;
    console.log(this.ListExam);

  }
}
