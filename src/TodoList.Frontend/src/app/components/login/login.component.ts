import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { first } from 'rxjs/operators';
import { UserLoginResponseModel } from 'src/app/models/authentication/userloginresponse.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  public currentUser: UserLoginResponseModel;
  public displayLoginFailedMessage = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.initLoginForm();

    this.authService.currentUser.subscribe(userLoginResponse => {
      this.currentUser = userLoginResponse;
    });
  }

  private initLoginForm(): void {
    this.loginForm = new FormGroup({
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'password': new FormControl(null, Validators.required)
    });
  }

  public onLogin(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.displayLoginFailedMessage = false;

    this.authService.login(this.loginForm.value.email, this.loginForm.value.password)
      .pipe(first())
      .subscribe(
        data => {
          console.log('Successfull login');
        },
        error => {
          if (error.status === 400) {
            this.displayLoginFailedMessage = true;
          }
        }
      );
  }

  public onLogout(): void {
    this.authService.logout();
  }
}
