import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthenTeacherGuard } from "../shared/services/authenteacher.guard";
import { AdminComponent } from "./admin.component";
import { AllExamComponent } from "./all-exam/all-exam.component";
import { AnalysisTestComponent } from "./analysis-test/analysis-test.component";
import { QuestionExamComponent } from "./question-exam/question-exam.component";
import { TestExamComponent } from "./test-exam/test-exam.component";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserComponent } from "./user/user.component";
import { AllQuestionComponent } from "./all-question/all-question.component";

const router : Routes =[
  {
    path : '',
    component : AdminComponent,
    children:[
      {
        path: '',
        redirectTo: '',
        pathMatch: 'full'
      },
      {
        path : 'user',
        component : UserComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Admin'
        }
      },
      {
        path : 'user-detail/:id',
        component : UserDetailComponent,
        // canActivate: [AuthenTeacherGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path : 'all-exam',
        component : AllExamComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
      {
        path : 'question-exam/:id',
        component : QuestionExamComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
      {
        path : 'test-exam/:id',
        component : TestExamComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
      {
        path : 'analysis-test/:id',
        component : AnalysisTestComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
      {
        path : 'all-question',
        component : AllQuestionComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
    ],
  },

]
@NgModule({
  imports : [RouterModule.forChild(router)],
  exports : [RouterModule]
})

export class AdminRoutingModule {

}
