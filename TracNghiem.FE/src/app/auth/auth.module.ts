import { NgModule } from "@angular/core";
import { AuthRoutingModule } from "./auth-routing.module";
import { RegisterComponent } from "./register/register.component";
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { CommonModule } from "@angular/common";


@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [ReactiveFormsModule,
    FormsModule,
    AuthRoutingModule,
    SharedModule,
    CommonModule ],
  exports: [RegisterComponent],
})

export class AuthModule{}
