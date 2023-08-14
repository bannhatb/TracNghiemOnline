import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponent } from './auth/auth.component';
import { CommonModule } from '@angular/common';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { HomeModule } from './home/home.module';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { NotificationService } from './shared/services/notification.service';
import { ForbidentComponent } from './shared/component/forbident/forbident.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    HomeComponent,
    AdminComponent,
    ForbidentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HomeModule,
    AuthModule,
    CommonModule,
    SharedModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [NotificationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
