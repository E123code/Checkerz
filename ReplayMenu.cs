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
            ReplayView.DataSource = TblBindingSource;

            DataGridViewCheckBoxColumn radioColumn = new DataGridViewCheckBoxColumn();
            radioColumn.Name = "Select Replay";
            radioColumn.Width = 50;

            radioColumn.ThreeState = false;
            ReplayView.Columns.Insert(0, radioColumn);
        }

        private void ReplayView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ReplayView.Columns["Select Replay"].Index && e.RowIndex >= 0)
            {

                foreach (DataGridViewRow row in ReplayView.Rows)
                {
                    row.Cells["Select Replay"].Value = false;
                }

                ReplayView.Rows[e.RowIndex].Cells["Select Replay"].Value = true;

                ReplayView.EndEdit();
            }
        }

        private void StartReplay_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in ReplayView.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select Replay"].Value) == true)
                {
                    selectedID = Convert.ToInt32(row.Cells["GameID"].Value);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("No game selected!");
        }
    }
}
