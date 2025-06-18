using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections.Generic;

namespace Api.Models
{
    public class Rede
    {
        public string IpRede { get; set; } = "10.3.192.";
        public int inicio { get; set; } = 0;
        public int fim { get; set; } = 0;
        public bool VarrerTodaRede { get; set; }
        public int qntThreads { get; set; }
        
        

        // --- Varredura Individual ---
        public GrupoDeIps varreduraIndividual(int inicio, int fim, int indiceVetorInicio)
        {
            Ping ping = new Ping();
            PingReply resposta;
            GrupoDeIps resultadoDaBusca = new GrupoDeIps();

            for (int i = inicio; i <= fim; i++)
            {
                Ip newIp = new Ip();
                string ipParaPingar = IpRede + i.ToString();
                newIp.ip = ipParaPingar;
                try
                {
                    resposta = ping.Send(ipParaPingar);
                    newIp.status = resposta.Status == IPStatus.Success ? 1 : 0;
                    resultadoDaBusca.ListaDeIps.Add(newIp);
                }
                catch (Exception err)
                {
                    Console.WriteLine("Erro: " + err);
                    break;
                }
            }

            return resultadoDaBusca;
        }

        public GrupoDeIps GatilhovarreduraIndividual()
        {
            Stopwatch cronometro = Stopwatch.StartNew();
            GrupoDeIps resultadoFinal = varreduraIndividual(inicio, fim, 0);
            cronometro.Stop();
            resultadoFinal.Tempo = cronometro.ElapsedMilliseconds / 1000.0;
            return resultadoFinal;
        }

        // --- Varredura com Threads Fixas ---
        public GrupoDeIps varreduraComThreads()
        {
            Stopwatch cronometro = Stopwatch.StartNew();

            int totalIps = fim - inicio + 1;
            int tamanhoBloco = totalIps / qntThreads;
            int resto = totalIps % qntThreads;

            int ipAtual = inicio;
            Thread[] threads = new Thread[qntThreads];
            GrupoDeIps[] resultadosParciais = new GrupoDeIps[qntThreads];

            for (int i = 0; i < qntThreads; i++)
            {
                int blocoInicial = ipAtual;
                int blocoTamanho = tamanhoBloco + (i < resto ? 1 : 0);
                int blocoFim = blocoInicial + blocoTamanho - 1;
                int indiceCopia = i;

                threads[i] = new Thread(() =>
                {
                    resultadosParciais[indiceCopia] = varreduraIndividual(blocoInicial, blocoFim, 0);
                });

                threads[i].Start();
                ipAtual += blocoTamanho;
            }

            foreach (var thread in threads)
                thread.Join();

            GrupoDeIps resultadoFinal = new GrupoDeIps();
            foreach (var resultado in resultadosParciais)
            {
                if (resultado != null)
                    resultadoFinal.ListaDeIps.AddRange(resultado.ListaDeIps);
            }

            cronometro.Stop();
            resultadoFinal.Tempo = cronometro.ElapsedMilliseconds / 1000.0;
            return resultadoFinal;
        }

        // --- Varredura com Threads Dinâmicas ---
        public GrupoDeIps varreduraComThreadsDinamicas()
        {
            Stopwatch cronometro = Stopwatch.StartNew();

            List<Thread> threads = new List<Thread>();
            List<GrupoDeIps> resultados = new List<GrupoDeIps>();
            object lockObj = new object();

            for (int i = inicio; i <= fim; i += 2)
            {
                int ipInicio = i;
                int ipFim = Math.Min(i + 1, fim);

                Thread t = new Thread(() =>
                {
                    GrupoDeIps resultado = varreduraIndividual(ipInicio, ipFim, 0);
                    lock (lockObj)
                    {
                        resultados.Add(resultado);
                    }
                });

                threads.Add(t);
                t.Start();
            }

            foreach (var thread in threads)
                thread.Join();

            GrupoDeIps resultadoFinal = new GrupoDeIps();
            foreach (var resultado in resultados)
                resultadoFinal.ListaDeIps.AddRange(resultado.ListaDeIps);

            cronometro.Stop();
            resultadoFinal.Tempo = cronometro.ElapsedMilliseconds / 1000.0;
            return resultadoFinal;
        }
    }
}
