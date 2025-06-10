namespace Api.Models
{
    public class Rede
    {
        public string IpRede { get; set; } = string.Empty;
        public int inicio{get; set; }
        public int fim{get; set; }
        public bool VarrerTodaRede { get; set; }
        public int qntThreads { get; set; }
    }
}