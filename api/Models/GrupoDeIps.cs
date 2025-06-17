using System.Runtime.InteropServices;

namespace Api.Models
{
    public class GrupoDeIps
    {
        public List<Ip> ListaDeIps { get; set; } = new List<Ip>();
        public double Tempo { get; set; } = 0;
    }
}