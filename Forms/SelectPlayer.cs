using CheckerZ.Data.DB;
using CheckerZ.Objects;
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
    // Class to handle player selection before a game starts
    public partial class SelectPlayer : Form
    {
        public SelectPlayer()
        {
            InitializeComponent();
        }

        public int selectedID { get; private set; }
        public string selectedName {  get; private set; }

        //Loads the current group of players in the session
        private void SelectPlayer_Load(object sender, EventArgs e)
        {
            PlayerBindingSource.DataSource = Session.Instance.Players;
            PlayerBindingNavigator.BindingSource = PlayerBindingSource;
            PlayerView.DataSource = PlayerBindingSource;

            DataGridViewCheckBoxColumn radioColumn = new DataGridViewCheckBoxColumn();
            radioColumn.Name = "Select Player";
            radioColumn.Width = 50;

            radioColumn.ThreeState = false;
            PlayerView.Columns.Insert(0, radioColumn);
        }

        // Selecting a player choosen by cell
        private void PlayerView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == PlayerView.Columns["Select Player"].Index && e.RowIndex >= 0)
            {

                foreach (DataGridViewRow row in PlayerView.Rows)
                {
                    row.Cells["Select Player"].Value = false;
                }

                PlayerView.Rows[e.RowIndex].Cells["Select Player"].Value = true;

                PlayerView.EndEdit();
            }
        }

        //Starts a game with player selected
        private void StartGame_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in PlayerView.Rows)
            {
                // Make sure the value isn't null before checking if it's true
                if (row.Cells["Select Player"].Value != null &&
                    Convert.ToBoolean(row.Cells["Select Player"].Value) == true)
                {
                    selectedID = Convert.ToInt32(row.Cells["Id"].Value);
                    selectedName = Convert.ToString(row.Cells["Name"].Value);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }

            // Changed "game" to "player" so the message makes sense to the user!
            MessageBox.Show("No player selected!");
        }
    }
}
