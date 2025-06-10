import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { from } from 'rxjs';

@Component({
  selector: 'app-home-page',
  imports: [ReactiveFormsModule,FormsModule,CommonModule],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css'
})
export class HomePage {

  constructor(private httpClient: HttpClient){
    this.get()

      this.form.get('VarrerTodaRede')?.valueChanges.subscribe((checked) => {
      const inicio = this.form.get('inicio');
      const fim = this.form.get('fim');
      if (checked) {
        inicio?.disable();
        fim?.disable();
       
      } else {
        inicio?.enable();
        fim?.enable();
      }
    });
  }

  form = new FormGroup({
    IpRede: new FormControl('',[Validators.required]),
    inicio: new FormControl('',[Validators.required]),
    fim: new FormControl('',[Validators.required]),
    VarrerTodaRede: new FormControl(false),
    qntThreads: new FormControl(0) //se a quantidade de trheads for igual a 7 é multithreads dinamicas 
  })
  
  
  onSubmit(Threads: number){
    this.form.controls.qntThreads.setValue(Threads)
    if(this.form.invalid) this.form.markAllAsTouched()
    console.log(this.form.value)
  }

  
  post(form: any) {
  const url = 'http://localhost:7041/api/produtos'; // Use HTTPS e o endpoint correto

  this.httpClient.post(url, form).subscribe({
    next: (response) => {
      console.log(response);
    },
    error: (err) => {
      console.error('Erro ao enviar:', err);
    }
  });
}


  get(){
   
    const url = 'http://localhost:5264/GetIps'
    this.httpClient.get(url).subscribe({
      
    })
  }
}
