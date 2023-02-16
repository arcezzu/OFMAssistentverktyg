namespace ÖFMSluträkningUI.DbModel {
    public class MainCategory : ICategory {
        public int id { get; set; }
        public string namn { get; set; } = string.Empty;

        public string GetMainCatNameByValue(int i) {

            return MainCatNameList[i];
        }

        private static readonly Dictionary<int, string> MainCatNameList = new Dictionary<int, string>() {

            { 1, "Inkomster"},
            { 2, "Utgifter"},
        };
    }
}
