import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthGuard } from "./auth.guard";
import { ExamService } from "./exam.service";
import decode, { JwtHeader, JwtPayload } from 'jwt-decode';
import { BaseService } from "./base.service";
//import * as jwt_decode from 'jwt-decode';
@Injectable({
  providedIn: 'root'
})
export class AuthenTeacherGuard implements CanActivate, CanActivateChild {
  constructor(private examService: ExamService,
    private auth: AuthGuard, public router: Router,private baseService: BaseService) {}
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var token1 = this.baseService.getToken();
    if(token1 == "" && token1== undefined && token1 == null){
      this.baseService.clearLocalStorage();
      this.router.navigate([`login`]);
      return false;
    }
    let expectedRole = childRoute.data.expectedRole;
    let token = localStorage.getItem('PBL7') || '';
    let tokenPayload : any = decode<JwtHeader>(token);
    console.log(tokenPayload);
    let role : string[] = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    console.log(role);
    if (
      !this.auth.isAuthenticated() ||
      !role.includes(expectedRole)
    ) {
      this.router.navigate(['forbident']);
      return false;
    }
    return true;
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var token1 = this.baseService.getToken();
    if(token1 == "" && token1== undefined && token1 == null){
      this.baseService.clearLocalStorage();
      this.router.navigate([`auth/login`]);
      return false;
    }
    let expectedRole = route.data.expectedRole;
    let token = localStorage.getItem('PBL7') || '';
    let tokenPayload : any = decode<JwtHeader>(token);
    console.log(tokenPayload);
    let role : string[] = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    console.log(role);
    if (
      !this.auth.isAuthenticated() ||
      !role.includes(expectedRole)
    ) {
      this.router.navigate(['forbident']);
      return false;
    }
    return true;
  }

}
