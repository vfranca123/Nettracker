using System;
using System.Diagnostics;
using System.Net.NetworkInformation;


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

        public GrupoDeIps ListaDePrintar= new GrupoDeIps();

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
                ipParaPingar = IpRede + i.ToString();
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
        public GrupoDeIps RetornaResultado()
        {
            for (int i = 0; i < resultadoDaBusca.Length; i += 2)
            {
                Ip ip = new Ip(); 
                ip.ip = IpRede + resultadoDaBusca[i];
                ip.status = resultadoDaBusca[i + 1] == 1 ? "Ativo" :
                                resultadoDaBusca[i + 1] == 0 ? "Inativo" : "Desconhecido";
                
                
                

                ListaDePrintar.ListaDeIps.Add(ip);

            }
            ListaDePrintar.Tempo = cronometro.ElapsedMilliseconds;
            return ListaDePrintar;
        }
    }
}
