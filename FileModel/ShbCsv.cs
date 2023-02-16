using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace ÖFMSluträkningUI.FileModel {
    public class Shb {

        [Index(0)]
        public DateTime Reskontradatum { get; set; }

        [Index(2)]
        public DateTime Transaktionsdatum { get; set; }

        [Index(4)]
        public string Text { get; set; } = string.Empty;

        [Index(6)]
        public decimal Belopp { get; set; } = 0;

        public Shb(DateTime reskontradatum, DateTime transaktionsdatum, string text, decimal belopp) {

            Reskontradatum = reskontradatum;
            Transaktionsdatum = transaktionsdatum;
            Text = text;
            Belopp = belopp;
        }
    }
}
