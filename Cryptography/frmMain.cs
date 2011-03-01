using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cryptography
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtOutput.Text = "";
                txtOutput.Text = Cryptography.Encrypt(txtInput.Text, txtSeed.Text);
            }
            catch
            { }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtOutput.Text = "";
                txtOutput.Text = Cryptography.Decrypt(txtInput.Text, txtSeed.Text);
            }
            catch 
            { }
        }
    }
}