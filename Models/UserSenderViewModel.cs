namespace PBL_Electronicrap.Models
{
    public class UserSenderViewModel : PadraoViewModel
    {
        public string username { get; set; }
        public string full_name { get; set; }
        public string cpf { get; set; }
        public string phone_number { get; set; }
        public string postal_code { get; set; }
        public string address_street { get; set; }
        public string address_number { get; set; }
        public string? address_complement { get; set; }
        public string address_district { get; set; }

        public string address_state { get; set; }
        public string address_city { get; set; }
        public string email { get; set; }
        public DateTime created_date { get; set; }

    }
}
