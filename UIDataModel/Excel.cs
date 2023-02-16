namespace ÖFMSluträkningUI.UIDataModel {
    internal static class ExcelData {

        public static(int, int) GetExcelDataColumnTarget(int catId) {

            var query = from row in Enumerable.Range(0, dataColumnPositionArray.GetLength(0))
                        where dataColumnPositionArray[row, 0] == catId
                        select (dataColumnPositionArray[row, 1], dataColumnPositionArray[row, 2]);

            return query.FirstOrDefault();
        }

        private static int[,] dataColumnPositionArray = new int[,] {

           {1, 5, 2}, // Lön
           {2, 5, 3}, // Aktivitets/sjukersättning
           {3, 5, 4}, // Pension från pensionsmyndighet
           {4, 5, 5}, // Pension övrig

           {5, 5, 8}, // Ränta
           {6, 5, 9}, // Skatteåterbäring
           {7, 5, 10}, // Bostadstillägg/bostadsbidrag
           {8, 5, 11}, // Merkostnadsersättning (handikappersättning)
           {9, 5, 12}, // Habiliteringsersättning (HAB)

           {10, 0, 0}, // Används ej  - Ekonomiskt bistånd

           {11, 5, 13}, // Inkomst av fastighet/arrende
           {12, 5, 14}, // Försäljning av fastighet, bostadsrätt
           {13, 5, 15}, // Försäljningslikvider, lösöre med mera

           {14, 5, 16}, // Sålda fondandelar eller aktier
           {15, 5, 17}, // Utbetald utdelning aktier/fonder
           {16, 5, 19}, // Barn-/studiebidrag

           {17, 5, 20}, // Arv/gåva

           // Utgifter start

           {18, 0, 0}, // Används ej - Överföringar från andra konton till godmanskontot

           {19, 4, 2}, // Preliminär skatt på inkomst

           {20, 4, 3}, // Skatt på ränta, utdelningar m.m
           {21, 4, 4}, // Kvarskatt
           {22, 4, 5}, // Fyllnadsinbetald skatt

           {23, 4, 6}, // Hyra
           {24, 4, 7}, // Hemtjänst, omsorgsavgift
           {25, 4, 8}, // El, fastighetsavgifter
           {26, 4, 9}, // Hemförsäkring

           {27, 4, 10}, // Övriga försäkringar
           {28, 4, 11}, // Personliga levnadskostnader
           {29, 4, 12}, // Läkemedel, läkarvård, tandvård
           {30, 4, 13}, // Inredning
           {31, 4, 14}, // Telefon, TV-avgift, bredband, tidningar m.m
           {32, 4, 15}, // Resor, färdtjänst

           {33, 4, 16}, // Bank- och postavgifter
           {34, 4, 17}, // Amortering av lån
           {35, 4, 18}, // Låneränta och -avgifter
           {36, 4, 19}, // Sparande i fonder och aktier

           {37, 4, 20}, // Kontanter till huvudmannen
           {38, 4, 21}, // Kontanter till boendet
           {39, 4, 22}, // Eget uttag av huvudmannen

           {40, 4, 23}, // Utbetalt arvode och kostnadsersättning enl. beslut
           {41, 4, 24}, // Inbetald skatt och sociala avgifter på arvode
           {42, 4, 25}, // Avbetalning skulder

           {43, 0, 0}, // Används ej - Överföringar från godmanskontot till andra konton
        };
    }
}
