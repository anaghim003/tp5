namespace RestoManager.Models.RestosModel
{
    public class Proprietaire
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }

        // Navigation property
        public ICollection<Restaurant> LesRestos { get; set; }
    }
}
