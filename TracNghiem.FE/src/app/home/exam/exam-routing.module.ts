import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthenTeacherGuard } from "src/app/shared/services/authenteacher.guard";
import { AddExamComponent } from "./add-exam/add-exam.component";
import { AnalysisResultComponent } from "./analysis-result/analysis-result.component";
import { CreateTestComponent } from "./create-test/create-test.component";
import { ExamDetailComponent } from "./exam-detail/exam-detail.component";
import { ExamComponent } from "./exam.component";
import { GenQuestionComponent } from "./gen-question/gen-question.component";
import { TestCreateComponent } from "./test-create/test-create.component";
import { TestExamComponent } from "./test-exam/test-exam.component";
import { AuthGuard } from "src/app/shared/services/auth.guard";

const router : Routes =[
  {
    path: '',
    children: [
      {
        path : 'question',
        loadChildren: () => import('./question/question.module').then(m=>m.QuestionModule)
      },
      {
        path : 'add-exam',
        component: AddExamComponent,
        canActivate: [AuthenTeacherGuard],
        data: {
          expectedRole: 'Teacher'
        }
      },
      {
        path : 'gen-question/:id',
        component: GenQuestionComponent,
        canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path : 'create-test/:id',
        component: CreateTestComponent,
        canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path: 'test-create',
        component: TestCreateComponent,
        canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path: 'analysis-result/:id',
        component: AnalysisResultComponent,
        canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path : 'exam-detail/:id',
        component: ExamDetailComponent,
        // canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path : 'test-exam/:id',
        component: TestExamComponent,
        canActivate: [AuthGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      },
      {
        path: '',
        component : ExamComponent,
        // canActivate: [AuthenTeacherGuard],
        // data: {
        //   expectedRole: 'Teacher'
        // }
      }
    ]
  },
  {
    path: '',
    redirectTo: 'exam',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forChild(router)],
  exports: [RouterModule]
})

export class ExamRoutingModule{}
