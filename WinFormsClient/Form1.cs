using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormsClient.localhost;

namespace WFClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Экземпляр прокси-класса для вызова матодов веб-сервиса 
        /// </summary>
        DBWebService dbws = new DBWebService();

        private void Form1_Load(object sender, EventArgs e)
        {
            ReloadForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadForm();
        }

        /// <summary> 
        /// Заполняет объект DataGridView данными 
        /// </summary>
        private void DisplayLetters()        {
            var a = dbws.GetMessageList();
            dataGridView1.DataSource = dbws.GetMessageList();
            dataGridView1.Columns.Remove("MessageId");
            dataGridView1.Columns.Remove("RecipientId");
            dataGridView1.Columns.Remove("SenderId");
        }

        /// <summary> 
        /// Задает имена получателей в ComboBox-e 
        /// </summary>
        private void SetRecipient()
        {            
            comboBox1.DataSource = dbws.GetEmployeeList();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "EmployeeId";
        }

        /// <summary> 
        /// Задает имена отправителей в ComboBox-e 
        /// </summary>
        private void SetSender()
        {
            comboBox2.DataSource = dbws.GetEmployeeList();
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "EmployeeId";
        }

        /// <summary> 
        ///Выолняет вставку нового пиьсма в таблицу
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (!Validate()) return;
            try
            {
                dbws.InsertMessage(new WinFormsClient.localhost.Message
                {
                    Content = textBox2.Text,
                    Date = dateTimePicker2.Value,
                    Title = textBox1.Text,
                    RecipientId = (int)comboBox1.SelectedValue,
                    SenderId = (int)comboBox2.SelectedValue
                });
                ReloadForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            errorProvider2.SetError(textBox2, "");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
        }

        /// <summary> 
        /// Производит проверку введенных через TextBox-ы параметров 
        /// </summary>
        private bool Validate()
        {
            if (textBox2.Text.Length > 1000)
            {
                errorProvider2.SetError(textBox2, "Сообщение должно содержать меньше 1000 символов");
                return false;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("sss");
                errorProvider2.SetError(textBox2, "Сообщение не может быть пустым");
                return false;
            }
            if (textBox1.Text.Length > 100)
            {
                errorProvider1.SetError(textBox1, "Тема сообщения должна содержать меньше 100 символов");
                return false;
            }
            if (textBox1.Text.Length == 0)
            {
                errorProvider1.SetError(textBox1, "Тема сообщения не может быть пустой");
                return false;
            }
            return true;
        }

        /// <summary> 
        /// Обновляет данные для формы 
        /// </summary>
        private void ReloadForm()
        {
            try
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = true;
                DisplayLetters();
                SetRecipient();
                SetSender();
            }
            catch (Exception ex)
            {
                groupBox2.Enabled = false;
                groupBox1.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
