using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormsClient.ServiceReference1;

namespace WinFormsClient
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
        //DBWebService dbs = new DBWebService();
        Service1Client dbs = new Service1Client();

        private void Form1_Load(object sender, EventArgs e)
        {
            var a = dbs.GetRoles("Aa");
            ReloadForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadForm();
        }

        /// <summary> 
        /// Задает имена получателей в ComboBox-e 
        /// </summary>
        private void SetRecipient()
        {
            comboBox1.DataSource = dbs.GetEmployeeList();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "EmployeeId";
        }

        /// <summary> 
        /// Задает имена отправителей в ComboBox-e 
        /// </summary>
        private void SetSender()
        {
            comboBox2.DataSource = dbs.GetEmployeeList();
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
                var a = new List<Entities.Recipient>();//////////////
                a.Add(new Entities.Recipient(5, 6,true));//////////////
                dbs.SendMessage(
                    new Entities.Message(
                        0,
                        textBox1.Text,
                        dateTimePicker2.Value,
                        a,
                        (Entities.Employee)comboBox2.SelectedItem,
                        textBox2.Text));
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
