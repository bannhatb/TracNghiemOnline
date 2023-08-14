import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { BaseService } from "./base.service";
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild{
constructor(private router: Router, private baseService: BaseService){

}
public isAuthenticated(){
  var token = this.baseService.getToken();
    if(token !== "" && token != undefined && token != null){
      return true;
    }
    else{
      return false;
    }
}


  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var token = this.baseService.getToken();
    if(token !== "" && token != undefined && token != null){
      return true;
    }
    else{
      this.baseService.clearLocalStorage();
      this.router.navigate([`login`]);
      return false;
    }
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    var token = this.baseService.getToken();
    if(token !== "" && token != undefined && token != null){
      return true;
    }
    else{
      this.baseService.clearLocalStorage();
      this.router.navigate([`auth/login`]);
      return false;
    }
  }

}
