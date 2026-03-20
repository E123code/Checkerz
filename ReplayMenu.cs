using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    public partial class ReplayMenu : Form
    {
        private ReplayDataDataContext dataContext = new ReplayDataDataContext();
        public int selectedID {  get; private set; }
        public ReplayMenu()
        {
            InitializeComponent();
        }

        private void ReplayMenu_Load(object sender, EventArgs e)
        {
            TblBindingSource.DataSource = dataContext.GameTables;
            TblBindingNavigator.BindingSource = TblBindingSource;
            ReplayView.DataSource = TblBindingNavigator;

            DataGridViewCheckBoxColumn radioColumn = new DataGridViewCheckBoxColumn();
            radioColumn.Name = "SelectRadio";
            //radioColumn.HeaderText = "בחירה";
            radioColumn.Width = 50;
            // ביטול ה-ThreeState כדי שיהיה רק "V" או כלום
            radioColumn.ThreeState = false;
            ReplayView.Columns.Insert(0, radioColumn);
        }

        private void ReplayView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // בדיקה שלחצנו על עמודת הבחירה ולא על הכותרת
            if (e.ColumnIndex == ReplayView.Columns["SelectRadio"].Index && e.RowIndex >= 0)
            {
                // 1. נקה את כל הסימונים הקיימים בטבלה
                foreach (DataGridViewRow row in ReplayView.Rows)
                {
                    row.Cells["SelectRadio"].Value = false;
                }

                // 2. סמן רק את השורה שנלחצה כרגע
                ReplayView.Rows[e.RowIndex].Cells["SelectRadio"].Value = true;

                // 3. עדכון מיידי של ה-Grid כדי שהמשתמש יראה את השינוי
                ReplayView.EndEdit();
            }
        }

        private void StartReplay_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in ReplayView.Rows)
            {
                if (Convert.ToBoolean(row.Cells["SelectRadio"].Value) == true)
                {
                    selectedID = Convert.ToInt32(row.Cells["Id"].Value);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("No game selected!");
        }
    }
}
