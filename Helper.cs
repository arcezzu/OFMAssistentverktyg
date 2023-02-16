using System.Configuration;

namespace ÖFMSluträkningUI {
    public static class Helper {
        public static string GetCnnVal(string name) {

            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
