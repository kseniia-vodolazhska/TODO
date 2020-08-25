import { Injectable } from '@angular/core';
import { RegistrationRequestModel } from '../models/registrationRequest.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable()
export class RegistrationService {
    constructor (private httpClient: HttpClient) {
    }

    public register(registrationRequest: RegistrationRequestModel) {
        return this.httpClient.post(`${environment.apiUrl}/api/user`, registrationRequest);
    }
}
