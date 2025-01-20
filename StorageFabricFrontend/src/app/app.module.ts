import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DeployComponent } from './components/deploy/deploy.component';
import { DestroyComponent } from './components/destroy/destroy.component';

@NgModule({
  declarations: [
    AppComponent,
    DeployComponent,
    DestroyComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }