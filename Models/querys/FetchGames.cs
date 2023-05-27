using NuGet.Protocol.Plugins;

namespace RentGameAPI.Models.querys
{
    public class FetchGames
    {
        public string atributo { get; set; }
        public string data { get; set; }


        FetchGames(string atributo, string data)
        {

            this.atributo = atributo;


            this.data = data;
        }
    }
}
