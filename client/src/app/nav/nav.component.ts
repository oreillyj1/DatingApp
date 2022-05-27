import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {} 
  //currentUser$: Observable<User>;

  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    //this.currentUser$=this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
      console.log(response);
 
    },error => {
      console.log('ERROR:' + error);
    })
    //console.log(this.model);
  }

  logout() { 
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }


}
