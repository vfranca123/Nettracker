using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;


namespace Api.Models
{
    public class Rede
    {
        public string IpRede { get; set; } = "10.3.192.";
        public int inicio { get; set; } = 0;
        public int fim { get; set; } = 0;
        public bool VarrerTodaRede { get; set; }
        public int qntThreads { get; set; }
        public int[] resultadoDaBusca;
        public Stopwatch cronometro;

        public GrupoDeIps ListaDePrintar = new GrupoDeIps();


        //Criação e contrução da varredura -----------------------------------------------------------------------
        public void InicializarVarreduda()
        {
            int quantidadeAVarrer = this.fim - this.inicio + 1;
            resultadoDaBusca = new int[2 * quantidadeAVarrer];

            int contador = this.inicio;
            for (int i = 0; i < 2 * quantidadeAVarrer; i += 2)
            {
                resultadoDaBusca[i] = contador;
                resultadoDaBusca[i + 1] = -1;
                contador++;
            }

            cronometro = new Stopwatch();
            cronometro.Start();
        }

        public void varreduraIndividual(int inicio, int fim, int indiceVetorInicio)
        {
            Ping ping = new Ping();
            PingReply resposta;
            string ipParaPingar = "";

            int posicaoVarreduraVetor = indiceVetorInicio;
            for (int i = inicio; i <= fim; i++)
            {
                ipParaPingar = IpRede + i.ToString();
                try
                {
                    resposta = ping.Send(ipParaPingar);
                    if (resposta.Status == IPStatus.Success)
                        resultadoDaBusca[posicaoVarreduraVetor + 1] = 1;
                    else
                        resultadoDaBusca[posicaoVarreduraVetor + 1] = 0;


                    posicaoVarreduraVetor += 2;
                }
                catch (Exception err)
                {
                    Console.WriteLine("Erro:"+ err);
                    return;
                }
            }

            cronometro.Stop();
        }

        //Varredura isolado -----------------------------------------------------------------------------------
        public void gatilhoVarrefuraSemThread()
        {
            InicializarVarreduda();
            varreduraIndividual(this.inicio, this.fim, 0);

        }

        public GrupoDeIps RetornaResultado()
        {
            ListaDePrintar.ListaDeIps.Clear();

            for (int i = 0; i < resultadoDaBusca.Length; i += 2)
            {
                Ip ip = new Ip();
                ip.ip = IpRede + resultadoDaBusca[i];
                ip.status = resultadoDaBusca[i + 1] == 1 ? "Ativo" : "--";
                                
                ListaDePrintar.ListaDeIps.Add(ip);

            }
            ListaDePrintar.Tempo = cronometro.ElapsedMilliseconds;
            return ListaDePrintar;
        }

        //Varredura com uma threa a mais ----------------------------------------------------------------------
        public void varreduraComThreads()
        {
            int divisaoTarefas = this.fim - this.inicio + 1;
            divisaoTarefas = divisaoTarefas / 2;
            int meioInicio = this.inicio + divisaoTarefas;

            Thread t = new Thread(() => varreduraIndividual(this.inicio + divisaoTarefas, this.fim,(meioInicio - this.inicio) * 2)); //a função lambda cria uma função que sera executada dentro da thread 
            varreduraIndividual(this.inicio, divisaoTarefas-1 , 0);
            t.Start();
        }
        
        public void gatilhoVarreduraComUmaThread()
        {
            InicializarVarreduda();
            varreduraComThreads();
        }

        
    }
}
