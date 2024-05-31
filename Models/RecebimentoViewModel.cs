
namespace PBL_Electronicrap.Models
{
    public class RecebimentoViewModel : PadraoViewModel
    {
        public string quantidade { get; set; }
        public string descricao { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public int categoria_lixo { get; set; }
    }
}
