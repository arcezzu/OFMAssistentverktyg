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
        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {

            if(dataGridView2.Rows[e.RowIndex].Cells[0].Value == null) {

                dataGridView2.Rows[e.RowIndex].Cells[0].Value = DataGridViewHelper.GetDataGridViewMaxCellNum(dataGridView2);
            }

            return;
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {

            System.Windows.Forms.DataGridViewTextBoxEditingControl de;

            try { de = (DataGridViewTextBoxEditingControl)e.Control; }
            catch(Exception) { 

            System.Windows.Forms.ComboBox editingComboBox;

            try { editingComboBox = (System.Windows.Forms.ComboBox)e.Control; }
            catch(Exception) { return; }

            if (editingComboBox != null) editingComboBox.SelectedIndexChanged += new System.EventHandler(this.editingComboBox_SelectedIndexChanged);
            }

            return;
        }
        private void editingComboBox_SelectedIndexChanged(object sender, System.EventArgs e) {

            DataAccess dAccess = new DataAccess();

            DataGridViewComboBoxEditingControl ec = (DataGridViewComboBoxEditingControl)sender;

            if(ec.SelectedValue is null) return;

            if(!Int32.TryParse(ec.SelectedValue.ToString(), out int valueId)) { return; }

            MainCategory hk = new MainCategory();
            SubCategory uk = new SubCategory();

            if (ec.DataSource != null) {

                DataGridViewComboBoxCell dc;

                if (ec.DataSource.GetType().ToString().IndexOf(hk.GetType().ToString()) != -1) {

                    List<SubCategory> uKat = dAccess.GetSqlSpSubCategoryByMainCatId(valueId);

                    dc = (DataGridViewComboBoxCell)dataGridView2.Rows[ec.EditingControlRowIndex].Cells[3];

                    dc.DataSource = uKat;
                }
                else if (ec.DataSource.GetType().ToString().IndexOf(uk.GetType().ToString()) != -1) {

                    List<DetailCategory> dKat = dAccess.GetSqlSpDetailCategoryBySubCatId(valueId);

                    dc = (DataGridViewComboBoxCell)dataGridView2.Rows[ec.EditingControlRowIndex].Cells[4];

                    dc.DataSource = dKat;
                }
            }

            return;
        }
    }
}
