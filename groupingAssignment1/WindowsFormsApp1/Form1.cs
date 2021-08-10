using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=PC-HYD-DEV01;Initial Catalog=assignment;Persist Security Info=True;User ID=sa;Password=Password123");
        SqlCommand com;
       
        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                com = new SqlCommand
                {
                    Connection = con,
                    CommandText = "select * from employee"
                };
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    foreach (DataColumn column in dt.Columns)
                    {
                        checkedListBox1.Items.Add(column.ColumnName, true);
                    }
                }
            }
            finally
            {
                con.Close();
            }
        }
        
        private void load_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand
                {
                    Connection = con,
                    CommandText = "select * from employee"
                };
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    table.DataSource = dt;
                }
            }
            finally
            {
                con.Close();
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {     
            try
            {
                textBox2.Clear();
                List<string> columnsSelected = new List<string>();
                string groupingColumns = string.Empty;
                com = new SqlCommand
                {
                    Connection = con,
                    CommandText = "select * from employee"
                };
                con.Open();
                foreach (var columnNames in checkedListBox1.CheckedItems)
                { 
                    columnsSelected.Add(columnNames.ToString());
                }
                SqlDataReader reader = com.ExecuteReader();
                DataTable dtEmployees = new DataTable();
                if (reader.HasRows)
                {
                    dtEmployees.Load(reader);
                    table.DataSource = dtEmployees;
                }
                
                DataView dvEmployees = dtEmployees.DefaultView;
                DataTable dtSortedData = dvEmployees.ToTable();
                DataTable uniqueCols = dvEmployees.ToTable(true,columnsSelected.ToArray());
                string[] selectClause=new string[uniqueCols.Rows.Count];
                int iterator = 0;
 
                foreach (DataRow drr in uniqueCols.Rows)
                {
                    foreach (DataColumn dcc in uniqueCols.Columns)
                    {                      
                        selectClause[iterator] = selectClause[iterator] + dcc.ColumnName + "=" + "'" + drr[dcc] + "'" +" "+ "and"+" ";
                    }
                    selectClause[iterator]=selectClause[iterator].Substring(0, selectClause[iterator].Length - 4);
                    iterator++;
                }
                string temp;
                for (int selectclausevalue = 0; selectclausevalue < selectClause.Length; selectclausevalue++)
                {
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.AppendText(selectClause[selectclausevalue]);
                    textBox2.AppendText(Environment.NewLine);
                
                    foreach (var rowsMatched in dtSortedData.Select(selectClause[selectclausevalue]))
                    { temp = string.Empty;
                        foreach (DataColumn dtc in dtSortedData.Columns)
                        {
                            temp = temp + rowsMatched[dtc]+" ";
                        }
                        textBox2.AppendText("\t");
                        textBox2.AppendText(temp);
                        textBox2.AppendText(Environment.NewLine);
                    }
                } 
            }
            finally
            {               
                con.Close();
            }
        }
    }
}





