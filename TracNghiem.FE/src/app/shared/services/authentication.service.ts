import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { localStorageKey } from "src/app/app.config";
import { BaseService } from "./base.service";

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService extends BaseService implements CanActivate{


  constructor(public httpClient: HttpClient, private router: Router){
    super(httpClient);
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if(this.getToken()){
      return true;
    }
    this.router.navigate(['/'], {
      queryParams: {returnUrl: state.url}
    })
    return false;
  }
  public Login(data: any) : Observable<any>{
    // return this.getnew('/en/api/categories')

    return this.post('/api/Account/login', data);
  }
  public RegisterUser(data : any){
    return this.post('/api/Account/register', data);
  }
  public Logout(): void{
    localStorage.removeItem(localStorageKey);
    this.clearLocalStorage();
  }
  public setToken(token :string){
    return localStorage.setItem('PBL7', token);
  }
}
