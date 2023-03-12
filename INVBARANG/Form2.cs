using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace INVBARANG
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }
        MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=;database=db_gg");


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Tampil(textBox1.Text);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Tampil("");
        }

        private void Tampil(string valueToSearch)
        {
            MySqlCommand command = new MySqlCommand("select * from master where concat(id, nama_barang, kategori, harga, exp) like '%" + valueToSearch + "%'", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.RowTemplate.Height = 60;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = table;
            DataGridViewImageColumn imgcol = new DataGridViewImageColumn();
            imgcol = (DataGridViewImageColumn)dataGridView1.Columns[5];
            imgcol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Nama BARANG";
            dataGridView1.Columns[2].HeaderText = "Kategori";
            dataGridView1.Columns[3].HeaderText = "Harga";
            dataGridView1.Columns[4].HeaderText = "Tanggal Kedaluarsa";
            dataGridView1.Columns[5].HeaderText = "Foto";
        }
        public void ExecMyQuery(MySqlCommand mcomd, string myMsg)
        {
            connection.Open();
            if (mcomd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show(myMsg);
            }
            else
            {
                MessageBox.Show("Error");
            }
            connection.Close();
            Tampil("");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.JPG;*.PNG;*.JPEG;*.GIF;)|*.jpg;*.png;*.jpeg;*.gif;";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();
            MySqlCommand command = new MySqlCommand("INSERT INTO master(nama_barang, kategori, harga, exp, foto) VALUES (@nama_barang,@kategori,@harga,@exp,@foto)", connection);
            command.Parameters.Add("@nama_barang", MySqlDbType.VarChar).Value = NB.Text;
            command.Parameters.Add("@kategori", MySqlDbType.VarChar).Value = KTG.Text;
            command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = H.Text;
            command.Parameters.Add("@exp", MySqlDbType.VarChar).Value = TK.Text;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Ditambahkan");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Byte[] img = (Byte[])dataGridView1.CurrentRow.Cells[7].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(ms);

            ID.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            NB.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            KTG.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            H.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            TK.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();
            MySqlCommand command = new MySqlCommand("UPDATE master SET nama_barang=@nama_barang,kategori=@kategori,harga=@harga,exp=@exp,foto=@foto WHERE id=@id", connection);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID.Text;
            command.Parameters.Add("@nama_barang", MySqlDbType.VarChar).Value = NB.Text;
            command.Parameters.Add("@kategori", MySqlDbType.VarChar).Value = KTG.Text;
            command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = H.Text;
            command.Parameters.Add("@exp", MySqlDbType.VarChar).Value = TK.Text;
            command.Parameters.Add("@foto", MySqlDbType.Blob).Value = img;

            ExecMyQuery(command, "Data Berhasil Diubah");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM master WHERE id=@id", connection);
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox2.Text;

            ExecMyQuery(command, "Data Berhasil Dihapus");
        }
    }
}
