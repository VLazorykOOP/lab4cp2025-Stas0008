using MySql.Data.MySqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadBooks();
        }
        private static string connectionString = "Server=localhost;Database=LibraryDB;Uid=root;Pwd=0000;";
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        private void LoadBooks()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Books";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView2.DataSource = dt.Copy();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading books: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Books (Library, Authors, Title, Publisher, PageCount, Genre, Price, Storage, BookNumber) " +
                                   "VALUES (@Library, @Authors, @Title, @Publisher, @PageCount, @Genre, @Price, @Storage, @BookNumber)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Library", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Authors", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Title", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Publisher", textBox4.Text);
                    cmd.Parameters.AddWithValue("@PageCount", Convert.ToInt32(textBox5.Text));
                    cmd.Parameters.AddWithValue("@Genre", textBox6.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(textBox7.Text));
                    cmd.Parameters.AddWithValue("@Storage", textBox8.Text);
                    cmd.Parameters.AddWithValue("@BookNumber", textBox9.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Book added successfully!");
                    LoadBooks();
                    ClearAddFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding book: " + ex.Message);
            }
        }

        private void ClearAddFields()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text =
            textBox5.Text = textBox6.Text = textBox7.Text = textBox8.Text = textBox9.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["Library"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["Authors"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Title"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["Publisher"].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells["PageCount"].Value.ToString();
                textBox6.Text = dataGridView1.SelectedRows[0].Cells["Genre"].Value.ToString();
                textBox7.Text = dataGridView1.SelectedRows[0].Cells["Price"].Value.ToString();
                textBox8.Text = dataGridView1.SelectedRows[0].Cells["Storage"].Value.ToString();
                textBox9.Text = dataGridView1.SelectedRows[0].Cells["BookNumber"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookID"].Value);
                    using (MySqlConnection conn = GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE Books SET Library = @Library, Authors = @Authors, Title = @Title, " +
                                       "Publisher = @Publisher, PageCount = @PageCount, Genre = @Genre, Price = @Price, " +
                                       "Storage = @Storage, BookNumber = @BookNumber WHERE BookID = @BookID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@BookID", bookId);
                        cmd.Parameters.AddWithValue("@Library", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Authors", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Title", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Publisher", textBox4.Text);
                        cmd.Parameters.AddWithValue("@PageCount", Convert.ToInt32(textBox5.Text));
                        cmd.Parameters.AddWithValue("@Genre", textBox6.Text);
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(textBox7.Text));
                        cmd.Parameters.AddWithValue("@Storage", textBox8.Text);
                        cmd.Parameters.AddWithValue("@BookNumber", textBox9.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Book updated successfully!");
                        LoadBooks();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating book: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Books WHERE Title LIKE @Title OR Authors LIKE @Authors";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", "%" + textBox10.Text + "%");
                    cmd.Parameters.AddWithValue("@Authors", "%" + textBox10.Text + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    adapter.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching books: " + ex.Message);
            }
        }
    }
}
