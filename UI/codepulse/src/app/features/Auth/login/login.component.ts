import { Component } from '@angular/core';
import { LoginRequest } from '../models/Login-request.model';
import { AuthService } from '../Services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

model: LoginRequest;

constructor(private authService:AuthService,private cookieService:CookieService,
            private router:Router
){
  this.model={
    email:'',
    password:''
  };
}
onFormSubmit(){
  this.authService.login(this.model)
  .subscribe({
    next:(response)=>{
      //Set Auth cookie
      this.cookieService.set('Authorization',`Bearer ${response.token}`,undefined,'/',undefined,true,'Strict');
      //Set the user
      this.authService.setUser({
        email:response.email,
        roles:response.roles
      });
      // Redirect in home page
      this.router.navigateByUrl('/'); 

    }
  })
  
}

}

