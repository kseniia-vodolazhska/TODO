import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RegistrationService } from 'src/app/services/registration.service';
import { RegistrationRequestModel } from 'src/app/models/registrationRequest.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})
export class RegisterComponent implements OnInit {
  public showRegistrationSuccessMessage: boolean;
  public automaticallyRedirectAfterSeconds = 3;
  public errors: string[] = [];

  public registrationForm: FormGroup;

  constructor(private registrationService: RegistrationService, private router: Router) { }

  ngOnInit() {
    this.initRegistrationForm();
  }

  public initRegistrationForm(): void {
    this.registrationForm = new FormGroup({
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'firstName': new FormControl(null, Validators.required),
      'lastName': new FormControl(null, Validators.required),
      'password': new FormControl(null, [Validators.required, Validators.minLength(8)])
    });
  }

  public onRegister(): void {
    if (this.registrationForm.invalid) {
      return;
    }

    this.errors = [];

    const registrationRequest = new RegistrationRequestModel();
    registrationRequest.email = this.registrationForm.value.email;
    registrationRequest.firstName = this.registrationForm.value.firstName;
    registrationRequest.lastName = this.registrationForm.value.lastName;
    registrationRequest.password = this.registrationForm.value.password;

    this.registrationService.register(registrationRequest).subscribe(
      () => {
        this.showRegistrationSuccessMessage = true;
        this.registrationForm.reset();
        setTimeout(() => {
          this.router.navigate(['/']);
        }, this.automaticallyRedirectAfterSeconds * 1000);
      },
      (errorResponse) => {
        if (errorResponse.status === 400 && errorResponse.error) {
          const validationErrors = errorResponse.error;
          for (const fieldName in validationErrors) {
            if (validationErrors.hasOwnProperty(fieldName)) {
              this.errors.push(validationErrors[fieldName]);
            }
          }
        }
      }
    );
  }
}
