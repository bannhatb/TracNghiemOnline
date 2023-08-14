import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { SharedModule } from "src/app/shared/shared.module";
import { DoTestComponent } from "./do-test/do-test.component";
import { TestRoutingModule } from "./test.routing.module";
import { UserPointComponent } from "./user-point/user-point.component";
import { UserTestComponent } from "./user-test/user-test.component";


@NgModule({
  declarations: [
    UserTestComponent,
    DoTestComponent,
    UserPointComponent
  ],
  imports: [
    SharedModule,
    TestRoutingModule,
    CommonModule,
    FormsModule,
  ],
  exports: []

})

export class TestModule{

}
