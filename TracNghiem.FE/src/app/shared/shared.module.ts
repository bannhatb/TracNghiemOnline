import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { NotFoundComponent } from "../not-found/not-found.component";
import { ComponentModule } from "./component/component.module";
import { CountDownComponent } from "./component/count-down/count-down.component";
import { FooterComponent } from "./component/footer/footer.component";
import { HeaderV2Component } from "./component/header-v2/header-v2.component";
import { HeaderComponent } from "./component/header/header.component";
import { NotificationComponent } from "./component/notification/notification.component";
import { PagingComponent } from "./component/paging/paging.component";
import { SearchComponent } from "./component/search/search.component";

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    SearchComponent,
    PagingComponent,
    NotificationComponent,
    CountDownComponent,
    HeaderV2Component,
    NotFoundComponent
  ],
  imports:[
    ComponentModule,
    RouterModule,
    FormsModule,
    CommonModule
  ],
  exports:[
    HeaderComponent,
    FooterComponent,
    SearchComponent,
    PagingComponent,
    NotificationComponent,
    CountDownComponent,
    HeaderV2Component,
    NotFoundComponent
  ]

})

export class SharedModule{}
