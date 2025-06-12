using System.Runtime.InteropServices;

namespace Api.Models
{
    public class GrupoDeIps
    {
        public List<Ip> ListaDeIps { get; set; } = new List<Ip>();
        public long Tempo { get; set; } = 0;

        public void Printa()
        {
            foreach (var ip in this.ListaDeIps)
            {
                Console.WriteLine(ip.ip);
                Console.WriteLine(ip.status);
            }
            Console.WriteLine(this.Tempo);
        }
        
    }
}