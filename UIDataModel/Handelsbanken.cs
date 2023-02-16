using System.Globalization;
using ÖFMSluträkningUI.FileModel;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using ÖFMSluträkningUI.Validation;

namespace ÖFMSluträkningUI.UIDataModel {
    internal class Functions {
        public static (List<Shb>, int) GetShbDataFromCsvFile(string csvFile) {

            int x=0;

            List<Shb> dataList = new List<Shb>();

            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("sv-SE");

                using(var reader = new CsvReader(new StreamReader(csvFile, System.Text.Encoding.GetEncoding(1252)), new CsvConfiguration(cultureInfo) { Delimiter = ";", HasHeaderRecord = false })) {

                while(reader.Read()) {

                    if(!DateTime.TryParse(reader.GetField<string>(0), out DateTime reskontradatum)) continue;

                    if(!DateTime.TryParse(reader.GetField<string>(2), out DateTime transaktionsdatum)) return (dataList, -1);

                    string text = reader.GetField<string>(4) ?? string.Empty;

                    if(!Decimal.TryParse(reader.GetField<string>(6), out Decimal belopp)) return (dataList, -1);

                    dataList.Add(new Shb(reskontradatum, transaktionsdatum, text, belopp));

                    x++;
                }
            }

            dataList.Reverse();

            return(dataList, x);
        }
    }
}
