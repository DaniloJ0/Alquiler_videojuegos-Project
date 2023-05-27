namespace RentGameAPI.Models.querys
{
    public class MenosRentado
    {
        public String name { get; set; }
        public int rango { get; set; }
        public int frecuencia { get; set; }

        public MenosRentado(string name,int rango,int frecuencia)
        {
            this.name = name;
            this.rango = rango;
            this.frecuencia=frecuencia;
        }
    }
}
