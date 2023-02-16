using ÖFMSluträkningUI;
using ÖFMSluträkningUI.UI;
using ÖFMSluträkningUI.DbModel;
using ÖFMSluträkningUI.FileModel;
using ÖFMSluträkningUI.UIDataModel;
using ÖFMSluträkningUI.Validation;

using Spire.Xls;

namespace ÖFMSluträkning
{
    public partial class Overview : Form {

        public Overview() {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            DataAccess dAccess = new DataAccess();

            if(!dAccess.TestDBConnection()) {

                MessageBox.Show("Kunde inte ansluta till SQL-server. Kontrollera att IP-adressen är tillåten och försök igen.");

                return;
            }

            ar_Txt.Text = (DateTime.Now.Year-1).ToString();

            SetDataGridViewDataSource(huvudkategori_Combo, dAccess.GetSqlSpMainCategory());
            SetDataGridViewDataSource(underkategori_Combo, dAccess.GetSqlSpSubCategory());
            SetDataGridViewDataSource(detaljkategori_Combo, dAccess.GetSqlSpDetailCategory());

            LoadTransactionTypesDataGridView(dAccess.GetSqlSpTransactionList());

            return;
        }

        private void bläddra_Btn_Click(object sender, EventArgs e) {

            using(OpenFileDialog openFileDialog = new OpenFileDialog()) {

                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "(*.csv)|*.csv|All files|(*.*)";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Välj Handelsbanken CSV-fil exporterad från Excel";

                if(openFileDialog.ShowDialog() == DialogResult.OK) {
                    
                    fil_Txt.Text = openFileDialog.FileName;
                }
            }
        }
        private void inlasning_Btn_Click(object sender, EventArgs e) {

            if(fil_Txt.Text.Length == 0) {

                MessageBox.Show("En fil måste anges ovan för att inläsning ska kunna äga rum. Välj \"Bläddra\" och sök upp CSV-filen");

                return;
            }

            if(!decimal.TryParse(ing_balans_Txt.Text, out decimal dSaldo)) {

                MessageBox.Show("Ange ett ingående saldo för året och försök igen.");

                return;
            }

            dataGridView1.Rows.Clear();

            (List<Shb> shbList, int res) = Functions.GetShbDataFromCsvFile(fil_Txt.Text);

            if(shbList.Count == 0 || res == -1) {

                MessageBox.Show("Ett fel uppstod när CSV-filens format skulle läsas in. Kontrollera filen och försök igen.");

                return;
            }

            DataAccess dAccess = new DataAccess();

            List<DetailCategory> ttypecatList = dAccess.GetSqlSpDetailCategory();

            transaktionstyp_Combo.ValueMember = "id";
            transaktionstyp_Combo.ValueType = typeof(System.Int32);
            transaktionstyp_Combo.DisplayMember = "namn";

            transaktionstyp_Combo.DataSource = ttypecatList;

            DataGridViewHelper dHelper = new DataGridViewHelper();

            for(int i=0;i<shbList.Count;i++) {

                dataGridView1.Rows.Add();

                dSaldo += shbList[i].Belopp;

                dataGridView1.Rows[i].Cells[0].Value = shbList[i].Reskontradatum.ToShortDateString();
                dataGridView1.Rows[i].Cells[1].Value = shbList[i].Transaktionsdatum.ToShortDateString();
                dataGridView1.Rows[i].Cells[2].Value = shbList[i].Text;
                dataGridView1.Rows[i].Cells[3].Value = shbList[i].Belopp;
                dataGridView1.Rows[i].Cells[4].Value = dSaldo.ToString();

                try { dataGridView1.Rows[i].Cells[5].Value = dHelper.GetTransactionTypeByRowData(shbList[i].Text, dataGridView2, 1, 4); }
                catch {}
            }

            return;
        }

        private void sparaTransaktionsrad_Btn_Click(object sender, EventArgs e) {

            DataAccess dataAccess = new DataAccess();

            if(!DataGridViewHelper.ValidateTransactionTypeRowData(dataGridView2)) {

                MessageBox.Show("Alla rader gällande transaktionstyper måste innehålla huvudkategori, underkategori och detaljkategori. Korrigera och försök igen.");

                return;
            }

            if(dataAccess.SqlSpDeleteTransactionTypeList() == -1) {

                MessageBox.Show("Kunde inte exekvera sqlSpDeleteTransactionTypeList. Vänligen kontakta ansvarig utvecklare med denna information.", "Internt allvarligt fel");

                return;
            }
            
            for(int x=0;x<dataGridView2.Rows.Count;x++) {

                if(DataGridViewHelper.ValidateDataGridRowValues(dataGridView2.Rows[x])) {

                    List<Grid.TransactionType> dataList = new List<Grid.TransactionType>();

                    dataList = DataGridViewHelper.TransactionTypeDataFromGridRow(dataGridView2.Rows[x]);

                    if(dataList.Count>0) dataAccess.SqlSpOfmInsertTransactionListDataRow(dataList);
                }
            }
        }

        private void godkann_Btn_Click(object sender, EventArgs e) {

            string target_excel_file = "Utskrift.xlsx";

            string income_excel_template_file = "inkomster.xlsx";
            string expense_excel_template_file = "utgifter.xlsx";

            if(!DataGridViewHelper.ValidateCsvTransactionTypeRowData(dataGridView1)) {

                MessageBox.Show("Alla rader måste innehålla reskontradatum och korrekt belopp (decimal). Korrigera och försök igen.");

                return;
            }

            if(!FormValidateUserInputData()) {

                MessageBox.Show("Alla värden angivna under konfiguration måste innehålla korrekta belopp (decimal). Korrigera och försök igen.");

                return;
            }

            try { File.Delete(target_excel_file); }
            catch(Exception) {

                MessageBox.Show($"Kunde inte radera utdatafil {target_excel_file}, används filen? Stäng den och försök igen.");

                return;
            }

            DataAccess dAccess = new DataAccess();

            List<DetailCategory> dcatList = dAccess.GetSqlSpDetailCategory();

            Workbook workbook = new Workbook();

            try { workbook.LoadFromFile(@"data\"+income_excel_template_file); } 
            catch(Exception) {

                MessageBox.Show($"Kunde inte ladda Excelmall från filen {income_excel_template_file} i programmets data-mapp, saknas filen i mappen? Korrigera och försök igen. Nuvarande mapp: " + Directory.GetCurrentDirectory().ToString());

                return;
            }

            if(!File.Exists(@"data\"+expense_excel_template_file)) {

                MessageBox.Show($"Kunde inte lokalisera Excelmall från filen {expense_excel_template_file} i programmets data-mapp, saknas filen i mappen? Korrigera och försök igen. Nuvarande mapp: " + Directory.GetCurrentDirectory().ToString());

                return;
            }

            MainCategory hk = new MainCategory();

            Worksheet worksheet = workbook.Worksheets.Add(hk.GetMainCatNameByValue(1));

            worksheet = workbook.Worksheets[0];

            foreach(DetailCategory d in dcatList) {

                for(int i=1;i<=12;i++) {

                    decimal sum = 0;

                    for(int c=0;c<dataGridView1.Rows.Count;c++) {

                        DateTime dt = Convert.ToDateTime(dataGridView1.Rows[c].Cells[0].Value);

                        int dc_val = Convert.ToInt32(dataGridView1.Rows[c].Cells[5].Value);

                        if(dc_val == d.id && dt.Month == i) sum += Convert.ToDecimal(dataGridView1.Rows[c].Cells[3].Value);
                    }

                    (int startrow, int startcol) = ExcelData.GetExcelDataColumnTarget(d.id);

                    int outrow = startrow + i-1;

                    // Hardcoded because certain rules apply to certain categories

                    switch(d.id) {

                        case 2: sum = Convert.ToDecimal(nuvarande_aktivitet_sjukersattning_Txt.Text);
                            break;
                        case 5 when outrow == 16: sum = Convert.ToDecimal(inkomst_ranta_Txt.Text);
                            break;
                        case 7:
                            sum = Convert.ToDecimal(bostadstillagg_Txt.Text);
                            break;
                        case 19:
                            sum = Convert.ToDecimal(preliminar_skatt_inkomst_Txt.Text);
                            break;
                        default:
                            break;
                    }

                    if(outrow > 0 && startcol > 0 && sum != 0) worksheet.Range[outrow, startcol].Value = string.Format("{0:0.00}", sum);
                }

                if(d.id == 18) {

                    // Create a new Excel worksheet at this point

                    Workbook workbook1 = new Workbook();

                    workbook1.LoadFromFile(@"data\"+expense_excel_template_file);

                    workbook.Worksheets.AddCopy(workbook1.Worksheets[0]);

                    worksheet = workbook.Worksheets[1];
                }
            }

            workbook.SaveToFile(target_excel_file);

            MessageBox.Show($"XLSX sparad med filnamn {target_excel_file}");

            return;
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {

            if(dataGridView2.Rows[e.RowIndex].Cells[0].Value == null) {

                dataGridView2.Rows[e.RowIndex].Cells[0].Value = DataGridViewHelper.GetDataGridViewMaxCellNum(dataGridView2);
            }

            return;
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {

            System.Windows.Forms.DataGridViewTextBoxEditingControl de;

            try { de = (DataGridViewTextBoxEditingControl)e.Control; }
            catch (Exception) {

                System.Windows.Forms.ComboBox editingComboBox;

                try { editingComboBox = (System.Windows.Forms.ComboBox)e.Control; }
                catch (Exception) { return; }

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

            if(ec.DataSource != null) {

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

            if(!DataValidation.TryParseDecimal(nuvarande_aktivitet_sjukersattning_Txt.Text)) return false;
            if(!DataValidation.TryParseDecimal(inkomst_ranta_Txt.Text)) return false;
            if(!DataValidation.TryParseDecimal(bostadstillagg_Txt.Text)) return false;
            if(!DataValidation.TryParseDecimal(preliminar_skatt_inkomst_Txt.Text)) return false;

            return true;
        }

        private void nyrad_Btn_Click(object sender, EventArgs e) {

            dataGridView2.Rows.Add();
        }

        private void avsluta_Btn_Click(object sender, EventArgs e) {

            Application.Exit();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) { }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e) { }
    }
}