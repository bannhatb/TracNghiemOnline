import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ErrorCode, Response } from 'src/app/shared/response.module';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  isSubmit: boolean = false;
  message : string = '';
  username : string = '';
  password : string = '';
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
  }

  onSubmit(): void{
    if(this.username.trim() == '' || this.password.trim() == ''){
      this.notificationService.error("Không được để trống");
    }
    var result : any;
    this.isSubmit = true;
    var param = {
      username: this.username,
      password: this.password
    };
    this.authenticationService
      .Login(param)
      .subscribe((response) => {
          result = response;
        }, err=>{
          console.log(err);
          this.message = err.error.message;
          if(this.message == ErrorCode.BadRequest){
            this.notificationService.error("Sai tài khoản hoặc mật khẩu");
          };
          if(this.message == ErrorCode.BlockUser){
            this.notificationService.warn("Tài khoản đang bị khóa!");
          }
        }, () =>{
          console.log(result);
          this.authenticationService.setToken(result.result.token);
          this.router.navigateByUrl('/');
        });
  }
}

