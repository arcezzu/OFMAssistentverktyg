namespace ÖFMSluträkningUI.DbModel {
    public class TransactionType {
        public int id { get; set; }
        public string transaktionstext { get; set; } = string.Empty;
        public int huvudkategori_id { get; set; }
        public int underkategori_id { get; set; }
        public int detaljkategori_id { get; set; }

    }
}
