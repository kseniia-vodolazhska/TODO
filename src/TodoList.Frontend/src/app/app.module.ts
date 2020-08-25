import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './components/header/header.component';
import { AuthService } from './services/authentication/auth.service';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { RegistrationService } from './services/registration.service';
import { HomeComponent } from './components/home/home.component';
import { TodoOverviewComponent } from './components/todo/todo-overview/todo-overview.component';
import { AuthGuard } from './services/authentication/auth.guard';
import { TodoListService } from './services/todolist.service';
import { JwtInterceptor } from './services/authentication/jwt.interceptor';
import { TodoEditComponent } from './components/todo/todo-edit/todo-edit.component';
import { ErrorInterceptor } from './services/error.interceptor';
import { ErrorComponent } from './components/error/error.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    TodoOverviewComponent,
    TodoEditComponent,
    ErrorComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    AuthService,
    RegistrationService,
    TodoListService
  ],
  entryComponents: [TodoEditComponent, ErrorComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
