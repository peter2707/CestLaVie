import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { AccountService } from './_services/account.service';
import { User } from './_model/user';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
	title = "C'est La Vie";

	constructor(
		private accountService: AccountService,
		@Inject(PLATFORM_ID) private platformId: Object
	) {}

	ngOnInit(): void {
		if (isPlatformBrowser(this.platformId)) {
			this.setCurrentUser();
		}
	}

	setCurrentUser() {
		const userString = localStorage.getItem('user');
		if(!userString) return;
		const user: User = JSON.parse(userString);
		this.accountService.setCurrentUser(user);
	}
}
