namespace RentGameAPI.Models.querys
{
    public class CambioPrecio
    {

        public int IdGame { get; set; }
        public  double RentalPrice{ get; set; }

        public CambioPrecio(int idGame, double rentalPrice)
        {
            this.IdGame = idGame;
            this.RentalPrice = rentalPrice;
        }


    }
}
