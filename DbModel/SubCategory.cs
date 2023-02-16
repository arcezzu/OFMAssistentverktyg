namespace ÖFMSluträkningUI.DbModel {
    public class SubCategory : ICategory {
        public int id { get; set; }
        public string namn { get; set; } = string.Empty;
        public int huvudkategori_id { get; set; }

    }
}
