import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeployComponent } from './components/deploy/deploy.component';
import { DestroyComponent } from './components/destroy/destroy.component';

const routes: Routes = [
  { path: '', redirectTo: 'deploy', pathMatch: 'full' }, 
  { path: 'deploy', component: DeployComponent },       
  { path: 'destroy', component: DestroyComponent },     
  { path: '**', redirectTo: 'deploy' }                
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}