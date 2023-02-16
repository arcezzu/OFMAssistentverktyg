namespace ÖFMSluträkningUI.DbModel {
    public class DetailCategory : ICategory {
        public int id { get; set; }
        public string namn { get; set; } = string.Empty;
        public int underkategori_id { get; set; }

    }
}
