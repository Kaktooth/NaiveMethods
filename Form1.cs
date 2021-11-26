using System;
using System.Data;
using System.Windows.Forms;

namespace NaiveMethods
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt;
            dt = GetDataGridViewAsDataTable(dataGridView1);
            double result = 0;
            int n = Convert.ToInt32(textBox1.Text);
            int rowLength = dt.Rows.Count - 1;
            string expression = "";
            for (int i = rowLength - n; i < rowLength; i++)
            {
                var yi = Convert.ToDouble(dt.Rows[i].ItemArray[1]);
                result += yi;
                if (i == rowLength - 1)
                {
                    expression += $"{yi}";
                }
                else
                {
                    expression += $"{yi}+";
                }
            }
            result = result / n;
            //dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dt.Rows.RemoveAt(rowLength);
            dt.Rows.Add(rowLength, result);
            dt.AcceptChanges();

            dataGridView1.DataSource = dt;
            listBox1.Items.Add($"{expression} / {n} = {result}");
        }
        private DataTable GetDataGridViewAsDataTable(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0) return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null) dtSource.Columns.Add(col.Name, typeof(string));
                    else dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    DataRow drNewRow = dtSource.NewRow();
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                    }
                    dtSource.Rows.Add(drNewRow);
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt;
            dt = GetDataGridViewAsDataTable(dataGridView1);

            DataTable dt2;
            dt2 = dt;
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "Exponential smothing";
            column.AutoIncrement = false;
            column.Caption = "Exponential smothing";
            column.ReadOnly = false;
            column.Unique = false;

            dt2.Columns.Add(column);
            double lastResult = 0;
            double result = 0;
            double a = Convert.ToDouble(textBox2.Text);

            int rowLength = dt.Rows.Count - 1;

            for (int i = 0; i < rowLength + 1; i++)
            {
                double yi = 0;
                if (i < rowLength)
                {
                    yi = Convert.ToDouble(dt.Rows[i].ItemArray[1]);
                }
                if (i == 1)
                {
                    double yi1 = Convert.ToDouble(dt.Rows[i - 1].ItemArray[1]);
                    result = yi1;
                    lastResult = result;
                }
                if (i >= 2)
                {
                    double yi1 = Convert.ToDouble(dt.Rows[i - 1].ItemArray[1]);
                    result = a * yi1 + (1 - a) * lastResult;
                    listBox1.Items.Add($" {a} * {yi1} + ({1 - a}) * {lastResult} = {result}");
                    lastResult = result;
                }

                dt2.Rows.Add(i, yi, result);
            }

            //dataGridView1.Rows.Clear();
            //dataGridView1.Columns.Clear();
            for (int i = 0; i < rowLength + 1; i++)
            {
                dt2.Rows.RemoveAt(0);
            }
            dt2.AcceptChanges();
            Data data = new Data();
            data.dataGridView1.DataSource = dt2;
            data.Show();
        }
    }
}
