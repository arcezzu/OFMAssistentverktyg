using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ÖFMSluträkningUI;
using ÖFMSluträkningUI.DbModel;
using ÖFMSluträkningUI.FileModel;
using ÖFMSluträkningUI.UIDataModel;
using ÖFMSluträkningUI.Validation;
using ÖFMSluträkningUI.UI;

namespace ÖFMSluträkning
{
    public partial class Overview
    {
            private void LoadTransactionTypesDataGridView(List<TransactionType> tcatList) {

            int c=0;

            foreach(TransactionType t in tcatList) {
                
                try {

                    dataGridView2.Rows.Add();

                    dataGridView2.Rows[c].Cells[0].Value = t.id;
                    dataGridView2.Rows[c].Cells[1].Value = t.transaktionstext;
                    dataGridView2.Rows[c].Cells[2].Value = t.huvudkategori_id;
                    dataGridView2.Rows[c].Cells[3].Value = t.underkategori_id;
                    dataGridView2.Rows[c].Cells[4].Value = t.detaljkategori_id;
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); return; }

                c++;
            }

            dataGridView2.AllowUserToAddRows = false;

            return;
        }

        private int SetDataGridViewDataSource<T>(DataGridViewComboBoxColumn dc, List<T> list) {

            dc.DisplayMember = "namn";
            dc.ValueMember = "id";

            dc.ValueType = typeof(System.Int32);

            dc.DataSource = list;

            return list.Count;
        }

        private bool FormValidateUserInputData() {

            if (!DataValidation.TryParseDecimal(nuvarande_aktivitet_sjukersattning_Txt.Text)) return false;
            if (!DataValidation.TryParseDecimal(inkomst_ranta_Txt.Text)) return false;
            if (!DataValidation.TryParseDecimal(bostadstillagg_Txt.Text)) return false;
            if (!DataValidation.TryParseDecimal(preliminar_skatt_inkomst_Txt.Text)) return false;

            return true;
        }
    }
}
