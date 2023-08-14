import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";
import { ExamRoutingModule } from "./exam-routing.module";
import { QuestionComponent } from "./question/question.component";
import { ExamDetailComponent } from './exam-detail/exam-detail.component';
import { AddExamComponent } from './add-exam/add-exam.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { GenQuestionComponent } from './gen-question/gen-question.component';
import { CreateTestComponent } from './create-test/create-test.component';
import { TestCreateComponent } from "./test-create/test-create.component";
import { AnalysisResultComponent } from "./analysis-result/analysis-result.component";
import { TestExamComponent } from './test-exam/test-exam.component';

@NgModule({
  declarations: [
    QuestionComponent,
    ExamDetailComponent,
    AddExamComponent,
    GenQuestionComponent,
    CreateTestComponent,
    TestCreateComponent,
    AnalysisResultComponent,
    TestExamComponent
  ],
  imports: [
    ExamRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    CommonModule
    ],
  exports: [],
})
export class ExamModule {

}
