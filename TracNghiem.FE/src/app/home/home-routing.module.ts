import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "../shared/services/auth.guard";
import { AuthenTeacherGuard } from "../shared/services/authenteacher.guard";
import { ExamComponent } from "./exam/exam.component";
import { HomeComponent } from "./home.component";

const router : Routes =[
  {
    path: '',
    children: [
      {
        path: 'exam',
        loadChildren: () => import('./exam/exam.module').then(m=>m.ExamModule),
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard]
      },
      {
        path: 'test',
        loadChildren: () => import('./test/test.module').then(m=>m.TestModule),
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard]
      },
      {
        path: '',
        component: HomeComponent,
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
  imports: [RouterModule.forChild(router)],
  exports: [RouterModule]
})

export class HomeRoutingModule{}
