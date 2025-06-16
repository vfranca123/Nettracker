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
        public int[] resultadoDaBusca = [];
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
                    Console.WriteLine("Erro:" + err);
                    return;
                }
            }


        }

        //Varredura isolado -----------------------------------------------------------------------------------
        public void gatilhoVarrefuraSemThread()
        {
            InicializarVarreduda();
            varreduraIndividual(this.inicio, this.fim, 0);
            cronometro.Stop();

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
            divisaoTarefas = divisaoTarefas / qntThreads;
            int resto = divisaoTarefas % qntThreads;

            int ipAtual = inicio;
            int indiceVetor = 0;
            Thread[] threads = new Thread[qntThreads];


            for (int i = 0; i < qntThreads; i++)
            {
                int blocoInicial = ipAtual;
                int blocoTamanho = divisaoTarefas + (1 < resto ? 1 : 0); // distribuira o resto não feito 
                int blocoFim = blocoInicial + blocoTamanho - 1;
                int vetorInicial = indiceVetor;
                threads[i] = new Thread(() => varreduraIndividual(blocoInicial, blocoFim, vetorInicial));
                varreduraIndividual(this.inicio, divisaoTarefas - 1, 0);
                threads[i].Start();

                ipAtual += blocoTamanho;
                indiceVetor += blocoTamanho * 2;
            }

            foreach (var thread in threads)
            {
                thread.Join(); // Espera todas as threads terminarem
            }

            cronometro.Stop();
        }

        public void gatilhoVarreduraComUmaThreadMais()
        {
            InicializarVarreduda();
            varreduraComThreads();
            cronometro.Stop();
        }


        //Varredura com threads dinamicas ----------------------------------------------------------------------
        public void varreduraComThreadsDinamicas()
        {
            int divisaoTarefas = this.fim - this.inicio + 1;
            divisaoTarefas = divisaoTarefas / qntThreads;
            int resto = divisaoTarefas % qntThreads;

            int ipAtual = inicio;
            int indiceVetor = 0;
        }
    }
}
