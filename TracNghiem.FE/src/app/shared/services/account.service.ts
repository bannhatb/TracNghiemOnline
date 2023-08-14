import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from 'rxjs';
import { environment } from "src/environments/environment";
import { BaseService } from "./base.service";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { UrlQuery } from "../Models/UrlQuery";
import { Response, Pagination, ResponseDefault } from "../response.module";

@Injectable({
  providedIn: 'root',
})
export class AccountService   extends BaseService implements CanActivate{
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    throw new Error("Method not implemented.");
  }
  get apiEndpoint(): string {
    return environment.API_ENDPOINT;
  }
  GetUserName() : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Home/get-username`);
  }

  GetUserDetailCurrent() : Observable<Response<ResponseDefault>>{
    return this.get(`/api/Home/get-user-detail-current`)
  }
}

