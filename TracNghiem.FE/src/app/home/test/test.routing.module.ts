import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/shared/services/auth.guard";
import { DoTestComponent } from "./do-test/do-test.component";
import { TestComponent } from "./test.component";
import { UserPointComponent } from "./user-point/user-point.component";
import { UserTestComponent } from "./user-test/user-test.component";

const router : Routes = [
  {
    path:'',
    children:[
      {
        path:'user-test/:id',
        component: UserTestComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'do-test/:testId',
        component: DoTestComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'user-point/:testId',
        component: UserPointComponent,
        canActivate: [AuthGuard]
      },
      {
        path: '',
        component: TestComponent,
        canActivate: [AuthGuard]
      }
  ]
  },
  {
    path: '',
    redirectTo: 'test',
    pathMatch: 'full'
  }

]

@NgModule({
  imports: [RouterModule.forChild(router)],
  exports: [RouterModule]
})
export class TestRoutingModule{

}
