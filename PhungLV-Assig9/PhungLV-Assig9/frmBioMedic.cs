using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PhungLV_Assig9
{
    public partial class frmBioMedic : Form
    {
        SqlConnection sqlcon;
        SqlDataAdapter sqlda;
        DataSet ds;
        public frmBioMedic()
        {
            InitializeComponent();
            sqlcon = new SqlConnection("Data Source=phunglv-PC;Initial Catalog=Patient;Integrated Security=SSPI");
            sqlda = new SqlDataAdapter("select * from PatientDetails",sqlcon);
            ds = new DataSet();
        }

        private void frmBioMedic_Load(object sender, EventArgs e)
        {
            sqlda.Fill(ds, "PatientDetails");
            SqlCommandBuilder buider = new SqlCommandBuilder(sqlda);
            dgridview.DataSource = ds.Tables["PatientDetails"];
            InsertCombobox();
        }

        void InsertCombobox()
        {
            cboSortBy.Items.Clear();
            for (int i = 0; i < ds.Tables["PatientDetails"].Columns.Count; i++)
                cboSortBy.Items.Add(ds.Tables["PatientDetails"].Columns[i].ToString());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchName.Text == "")
            {
                MessageBox.Show("Please enter the Name.", "BioMedic",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearchName.Focus();
            }
            else
            {
                DataView dvw = new DataView();
                dvw.Table = ds.Tables["PatientDetails"];
                dvw.RowFilter = "PatientName like '" + txtSearchName.Text + "%'";
                if (dvw.Count == 0)
                {
                    MessageBox.Show("Find Not found.", "BioMedic",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSearchName.SelectAll();
                    txtSearchName.Focus();
                }
                else
                {
                    dgridview.DataSource = dvw;
                }
                
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            dgridview.DataSource = ds.Tables["PatientDetails"].DefaultView;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgridview.SelectedRows.Count > 0)
            {

                if (MessageBox.Show("Do you want to delete?", "BioMedic", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dgridview.SelectedRows)
                    {
                        ds.Tables["PatientDetails"].Rows[row.Index].Delete();
                        
                    }
                    sqlda.Update(ds, "PatientDetails");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgridview.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgridview.SelectedRows)
                {
                    ds.Tables["PatientDetails"].Rows[row.Index]["PatientName"] = ds.Tables["PatientDetails"].Rows[row.Index]["PatientName"].ToString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["PatientAge"] = ds.Tables["PatientDetails"].Rows[row.Index]["PatientAge"].ToString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["PatientAddress"] = ds.Tables["PatientDetails"].Rows[row.Index]["PatientAddress"].ToString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["PatientCountry"] = ds.Tables["PatientDetails"].Rows[row.Index]["PatientCountry"].ToString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["DiagnosticTest"] = ds.Tables["PatientDetails"].Rows[row.Index]["DiagnosticTest"].ToString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["DateOfTesting"] = Convert.ToDateTime(ds.Tables["PatientDetails"].Rows[row.Index]["DateOfTesting"].ToString()).ToShortDateString();
                    ds.Tables["PatientDetails"].Rows[row.Index]["DateOfReport"] = Convert.ToDateTime(ds.Tables["PatientDetails"].Rows[row.Index]["DateOfReport"].ToString()).ToLongDateString(); ;
                    //MessageBox.Show(ds.Tables["PatientDetails"].Rows[row.Index]["DateOfTesting"].ToString());
                }
                sqlda.Update(ds, "PatientDetails");
            }
        }

        private void cboSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSortBy.SelectedIndex >= 0)
            {
                DataView dvw = new DataView();
                dvw = (DataView)dgridview.DataSource;
                dvw.Sort = cboSortBy.SelectedItem.ToString();
                dgridview.DataSource = dvw;
            }
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter the name.", "BioMedic",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtAge.Text = "";
            cboCountry.Text = "";
            cboTest.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow drow = ds.Tables["PatientDetails"].NewRow();
            drow["PatientName"] = txtName.Text;
            drow["PatientAge"] = Convert.ToInt32(txtAge.Text);
            drow["PatientAddress"] = txtAddress.Text;
            drow["PatientCountry"] = cboCountry.Text;
            drow["DiagnosticTest"] = cboTest.Text;
            drow["DateOfTesting"] = dtpDateOfTesting.Text;
            drow["DateOfReport"] = dtpDateOfReport.Text;
            ds.Tables["PatientDetails"].Rows.Add(drow);
            sqlda.Update(ds, "PatientDetails");
            ds.Tables["PatientDetails"].Clear();
            frmBioMedic_Load(null, null);
        }
    }
}