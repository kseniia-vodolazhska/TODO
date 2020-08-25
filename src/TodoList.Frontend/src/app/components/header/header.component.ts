import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { UserLoginResponseModel } from 'src/app/models/authentication/userloginresponse.model';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  public isAuthenticated = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.currentUser.subscribe((user: UserLoginResponseModel) => {
      this.isAuthenticated = user != null;
    });
  }
}