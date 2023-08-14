import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from '../../services/auth.guard';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-header-v2',
  templateUrl: './header-v2.component.html',
  styleUrls: ['./header-v2.component.scss']
})
export class HeaderV2Component implements OnInit {

  constructor(private checkAuthor: AuthGuard,
    private authen: AuthenticationService , public router: Router) {
  }

  ngOnInit(): void {
    this.check = this.checkAuthor.isAuthenticated();
  }
  check:boolean = false;
  logout1(){
    this.authen.Logout();
    this.router.navigateByUrl(`/login`);
  }

}
