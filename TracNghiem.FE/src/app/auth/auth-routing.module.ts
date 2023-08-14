import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";

const route: Routes=[
  {
    path: '',
    children: [
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      },
    ]
  },
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  },

];
@NgModule({
  imports: [RouterModule.forChild(route)],
  exports: [RouterModule]
})
export class AuthRoutingModule{}
