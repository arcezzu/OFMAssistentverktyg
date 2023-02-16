using ÖFMSluträkningUI.Validation;
using ÖFMSluträkningUI.UIDataModel;

namespace ÖFMSluträkningUI.UI {
    public class DataGridViewHelper {
        public int GetTransactionTypeByRowData(string value, DataGridView dataGridView, int colIndexVal, int colIndexType) {

            var query = from DataGridViewRow dr in dataGridView.Rows
                        where dr.Cells[colIndexVal].Value.ToString() is string cellValue &&
                        (cellValue.Contains(value) == true || value.Contains(cellValue))
                        select new { TransactionType = dr.Cells[colIndexType].Value };
             

            return query.FirstOrDefault()?.TransactionType as int? ?? 0;
        }

        public static List<Grid.TransactionType> TransactionTypeDataFromGridRow(DataGridViewRow dataGridViewRow) {

            List<Grid.TransactionType> transactionTypeDataList = new List<Grid.TransactionType>();

            transactionTypeDataList.Add(new Grid.TransactionType {

                Value1 = dataGridViewRow.Cells[1].Value.ToString(),
                Value2 = Convert.ToInt32(dataGridViewRow.Cells[2].Value),
                Value3 = Convert.ToInt32(dataGridViewRow.Cells[3].Value),
                Value4 = Convert.ToInt32(dataGridViewRow.Cells[4].Value)
            });

            return transactionTypeDataList;
        }
        public static int GetDataGridViewMaxCellNum(DataGridView dataGridView) {

            int m=1;

            foreach (DataGridViewRow row in dataGridView.Rows) {

                if(row.Cells[0].Value == null) return m+1;
                else m = Convert.ToInt32(row.Cells[0].Value);
            }

            return m;
        }
        public static bool ValidateDataGridRowValues(DataGridViewRow dataGridViewRow) {

            for(int i=0;i<5;i++) if (dataGridViewRow.Cells[i].Value == null) return false;

            return true;
        }
        public static bool ValidateCsvTransactionTypeRowData(DataGridView dataGridView) {

            for(int c=0;c<dataGridView.Rows.Count;c++) {

                if (dataGridView.Rows[c].Cells[0].Value is null) return false;
                if (dataGridView.Rows[c].Cells[3].Value is null) return false;

                if (!DataValidation.TryParseDateTime(dataGridView.Rows[c].Cells[0].Value.ToString())) return false;
                if (!DataValidation.TryParseDecimal(dataGridView.Rows[c].Cells[3].Value.ToString())) return false;
            }

            return true;
        }
        public static bool ValidateTransactionTypeRowData(DataGridView dataGridView) {

            for(int x=0;x<dataGridView.Rows.Count;x++) {

                if (dataGridView.Rows[x].Cells[4].Value == null) return false;

                if (!DataValidation.TryParseInt32(dataGridView.Rows[x].Cells[2].Value.ToString())) return false;
                if (!DataValidation.TryParseInt32(dataGridView.Rows[x].Cells[3].Value.ToString())) return false;
                if (!DataValidation.TryParseInt32(dataGridView.Rows[x].Cells[4].Value.ToString())) return false;
            }

            return true;
        }
    }
}
