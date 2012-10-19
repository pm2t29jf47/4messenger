using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestWinFormsClient.ServiceReference1;

namespace TestWinFormsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Service1Client dbs = new Service1Client();

        private void loginButton_Click(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {
           
            ///переделывать под текущую бд
            var a = new List<Entities.Recipient>();
            a.Add(new Entities.Recipient(5, 6, true));
        //    dbs.SendMessage(
        //        new Entities.Message(
        //            0,
        //            "Заголовок",
        //            DateTime.Now,
        //            a,
        //            new Entities.Employee(1,"sss"),
        //            "Текст сообщения"));
        }

        
    }
}
