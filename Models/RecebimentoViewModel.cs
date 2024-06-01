
namespace PBL_Electronicrap.Models
{
    public class RecebimentoViewModel : PadraoViewModel
    {
        public string quantidade { get; set; }
        public string descricao { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public int categoria_lixo { get; set; }
        public DateTime data { get; set; }

        //campo join
        public string categoria_lixo_descricao { get; set; }
        public string sender_full_name  { get; set; }
        public string sender_cpf { get; set; }
    }
}
