import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { HomeRoutingModule } from "./home-routing.module";
import { ExamComponent } from './exam/exam.component';
import { TestComponent } from './test/test.component';
import { CommonModule } from "@angular/common";

@NgModule({
  declarations: [
    ExamComponent,
    TestComponent,
  ],
  imports:[
    HomeRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  exports: []
})

export class HomeModule {

}

