import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserLoginRequestModel } from 'src/app/models/authentication/userloginrequest.model';
import { UserLoginResponseModel } from 'src/app/models/authentication/userloginresponse.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthService {
    private currentUserSubject: BehaviorSubject<UserLoginResponseModel>;
    public currentUser: Observable<UserLoginResponseModel>;

    constructor (private httpClient: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<UserLoginResponseModel>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): UserLoginResponseModel {
        return this.currentUserSubject.value;
    }

    public login(email: string, password: string) {
        const userLoginRequest = new UserLoginRequestModel(email, password);

        return this.httpClient.post<UserLoginResponseModel>(`${environment.apiUrl}/api/user/login`, userLoginRequest)
            .pipe(map(userLoginResponse => {
                if (userLoginResponse.succeeded) {
                    localStorage.setItem('currentUser', JSON.stringify(userLoginResponse));
                    this.currentUserSubject.next(userLoginResponse);
                }

                return userLoginResponse;
            }));
    }

    public logout(): void {
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}