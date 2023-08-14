import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { AdminRoutingModule } from "./admin-routing.module";
import { UserComponent } from './user/user.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { AllExamComponent } from './all-exam/all-exam.component';
import { QuestionExamComponent } from './question-exam/question-exam.component';
import { TestExamComponent } from './test-exam/test-exam.component';
import { AnalysisTestComponent } from './analysis-test/analysis-test.component';
import { AllQuestionComponent } from './all-question/all-question.component';

@NgModule({
  imports: [SharedModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AdminRoutingModule
  ],
  exports: [],
  declarations: [
    UserComponent,
    UserDetailComponent,
    AllExamComponent,
    QuestionExamComponent,
    TestExamComponent,
    AnalysisTestComponent,
    AllQuestionComponent,
  ]
})
export class AdminModule {}
