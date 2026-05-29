import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
 import { User } from '../models/user';
import { of, switchMap } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
 baseUrl = environment.baseUrl;
  private http = inject(HttpClient);
   currentUser = signal<User | null>(null);
  isAdmin = computed(() => {
    const roles = this.currentUser()?.roles;
    return Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin'
  })

  login(values: any) {
  
    let params = new HttpParams();
    params = params.append('useCookies', 'true');
    
     return this.http.post<{ message?: string }>(this.baseUrl + 'user/login', values, { withCredentials: true }).pipe(
      switchMap(() => this.getCurrentUser())
    );
  }
  
  logout() {
  return this.http.post(this.baseUrl + 'user/logout', {}, { withCredentials: true }).pipe(
    tap(() => this.currentUser.set(null))
  );
}


  getUserInfo() {
     throw new Error('getUserInfo() requires an email. Use getUserByEmail(email).');
  }

  

 

   

  

  getCurrentUser() {
     
    return this.http.get<User>(this.baseUrl + 'user/me', { withCredentials: true }).pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    );
  }

}
