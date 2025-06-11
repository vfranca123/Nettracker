using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Api.Models
{
    public class Rede
    {
        public string IpRede { get; set; } = "";
        public int inicio { get; set; } = 0;
        public int fim { get; set; } = 0;
        public bool VarrerTodaRede { get; set; }
        public int qntThreads { get; set; }

        public string IpBase = "10.3.192."; 
        public int[] resultadoDaBusca;
        public Stopwatch cronometro;

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

        public void varreduraSemThread()
        {
            Ping ping = new Ping();
            PingReply resposta;
            string ipParaPingar = "";

            int posicaoVarreduraVetor = 1;
            for (int i = this.inicio; i <= this.fim; i++)
            {
                ipParaPingar = IpBase + i.ToString();
                try
                {
                    resposta = ping.Send(ipParaPingar);
                    if (resposta.Status == IPStatus.Success)
                        resultadoDaBusca[posicaoVarreduraVetor] = 1;
                    else
                        resultadoDaBusca[posicaoVarreduraVetor] = 0;

                    posicaoVarreduraVetor += 2;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao fazer ping em {ipParaPingar}: {ex.Message}");
                    return;
                }
            }

            cronometro.Stop();
        }

        public void gatilhoVarrefuraSemThread()
        {
            InicializarVarreduda();
            varreduraSemThread();
        }

        //feito para teste da varredura simples
        public void ExibirResultados()
        {
            for (int i = 0; i < resultadoDaBusca.Length; i += 2)
            {
                string ip = IpBase + resultadoDaBusca[i];
                string status = resultadoDaBusca[i+1] == 1 ? "Ativo" :
                                resultadoDaBusca[i+1] == 0 ? "Inativo" : "Desconhecido";
                Console.WriteLine($"{ip} => {status}");
            }
            Console.WriteLine($"Tempo total: {cronometro.ElapsedMilliseconds} ms");
        }
    }
}
