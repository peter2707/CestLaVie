import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_model/member';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root'
})
export class MembersService {
	baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) { }

	getMembers() {
		return this.http.get<Member[]>(this.baseUrl + 'users');
	}

	getMember(username: string) {
		return this.http.get<Member>(this.baseUrl + 'users/' + username);
	}

}
