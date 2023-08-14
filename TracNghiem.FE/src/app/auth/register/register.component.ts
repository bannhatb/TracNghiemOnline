import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { TestService } from 'src/app/shared/services/test.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  formRegister : FormGroup;
  constructor(private testService: TestService, private activeRoute: ActivatedRoute,
    private fb : FormBuilder,
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService) { }
  classData : any;
  Result : any;
  ngOnInit(): void {
    this.GetAllClass();
    this.formRegister = this.fb.group({
      userName : [''],
      email: [''],
      password : [''],
      confirmPassword : [''],
      classId : ['']
    });
  }
  GetAllClass(){
    this.testService.GetAllClass().subscribe((res)=>{
      this.classData = res.result?.data;
      console.log(this.classData);
    }, (err)=>{
      console.log(err.error.message);
    })
  }
  submit(){
    let requestModel= {
      userName : this.formRegister.value.userName,
      email: this.formRegister.value.email,
      password : this.formRegister.value.password,
      verifyPassword : this.formRegister.value.confirmPassword,
      classId : this.formRegister.value.classId
    }
    if(requestModel.userName =="" || requestModel.email == ""
    || requestModel.password == "" || requestModel.verifyPassword ==  ""){
      this.notificationService.info("Vui lòng nhập đủ thông tin");
    }
    if(requestModel.verifyPassword != requestModel.password){
      this.notificationService.info("Mật khẩu không khớp");
    }
    this.authenticationService.RegisterUser(requestModel).subscribe((res)=>{
      this.Result = res;
      console.log(res);
      if(this.Result.result?.data != "00000000"){
        this.notificationService.info("Có lỗi xảy ra");
      }
      if(this.Result.message== "00000000"){
        this.notificationService.success("Đăng kí thành công");
      }
    }, (err)=>{
      console.log(err.error.message);
    })
  }

}
