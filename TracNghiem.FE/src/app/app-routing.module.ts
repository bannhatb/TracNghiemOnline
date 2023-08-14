import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { ForbidentComponent } from './shared/component/forbident/forbident.component';
import { AuthenTeacherGuard } from './shared/services/authenteacher.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m=>m.AuthModule)
  },

  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m=>m.AdminModule),
    // canActivate: [AuthenTeacherGuard],
    // data: {
    //   expectedRole: 'Teacher'
    // }
  },
  {
    path: 'forbident',
    component: ForbidentComponent
  },
  {
    path: '',
    loadChildren: () => import('./home/home.module').then(m=>m.HomeModule),
  },
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  },
  // {path: '404', component: NotFoundComponent},
  // {path: '**', redirectTo: '/404'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      preloadingStrategy: PreloadAllModules
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
