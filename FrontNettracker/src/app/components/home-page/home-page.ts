import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-home-page',
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.css']  // plural aqui
})
export class HomePage {
  carregando = false;
  resultado: { listaDeIps: any[]; tempo: number } = { listaDeIps: [], tempo: 0 };


  form = new FormGroup({
    IpRede: new FormControl('', [Validators.required]),
    inicio: new FormControl<number>(0, [Validators.required]),
    fim: new FormControl<number>(0, [Validators.required]),
    VarrerTodaRede: new FormControl(false),
    qntThreads: new FormControl(0)
  });

  constructor(private httpClient: HttpClient) {
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

    if (this.form.controls['VarrerTodaRede'].value === true) {
      this.form.controls['inicio'].setValue(0);
      this.form.controls['fim'].setValue(255);
    }
    this.form.controls['IpRede'].setValue('10.3.192.');
  }

  onSubmit(Threads: number) {
    this.form.controls.qntThreads.setValue(Threads);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;  // PARA A EXECUÇÃO SE FORM INVÁLIDO
    }
    this.post(this.form.value);
  }

 
  async post(form: any) {
    const url = 'http://localhost:5066/api/controller';
    this.carregando = true;
    try {
      const response: any = await firstValueFrom(
      this.httpClient.post(url, form, { responseType: 'json' })
      );

      this.resultado = response;
      console.log(this.resultado);
    } catch (err) {
      console.error('Erro ao enviar:', err);
    } finally {
      this.carregando = false;
    }
  }

  async get() {
    const url = 'http://localhost:5066/api/controller';
    try {
      const data = await firstValueFrom(this.httpClient.get(url));
      console.log('GET recebido:', data);
    } catch (err) {
      console.error('Erro no GET:', err);
    }
  }
}
