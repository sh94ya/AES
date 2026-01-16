using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace AES
{
    public partial class Form1 : Form
    {
        FileStream fs = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox1.MaxLength = 16;
            textBox2.MaxLength = 32;
            textBox6.MaxLength = 16;
            textBox5.MaxLength = 32;
            textBox17.MaxLength = 32;
            textBox18.MaxLength = 16;
            textBox14.MaxLength = 32;
            textBox15.MaxLength = 16;
            comboBox1.Items.Add("128");
            comboBox1.SelectedIndex = 0;
            comboBox1.Items.Add("192");
            comboBox1.Items.Add("256");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox9.Text = "";
            textBox8.Text = "";
            byte[,] state = new byte[4, 4];
            int k=0;
            int Nk=0;
            if (comboBox1.SelectedIndex == 0) { Nk = 4; }
            if (comboBox1.SelectedIndex == 1) { Nk = 6; }
            if (comboBox1.SelectedIndex == 2) { Nk = 8; }
            byte[][] key = new byte[Nk][];
            for (int i = 0; i < Nk; i++)
            {
                key[i] = new byte[4];
            }
                if ((textBox2.Text == "" || textBox5.Text == "") && fs == null) { MessageBox.Show("Заполните поля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {
                    try
                    {
                        for (int i = 0; i < Nk; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                string temp = Convert.ToString(textBox2.Text[k]) + Convert.ToString(textBox2.Text[k + 1]);
                                key[i][j] = Convert.ToByte(temp, 16);
                                k += 2;
                            }
                        }
                        k = 0;
                        //шифрование текста
                        if (fs == null)
                        {
                            for (int i = 0; i < 4; i++)
                                for (int j = 0; j < 4; j++)
                                {
                                    string temp = Convert.ToString(textBox5.Text[k]) + Convert.ToString(textBox5.Text[k + 1]);
                                    state[j, i] = Convert.ToByte(temp, 16);
                                    k += 2;
                                }
                            AES aes = new AES(state, key);
                            byte[,] state1 = aes.Encrypt();
                            for (int i = 0; i < 4; i++)
                                for (int j = 0; j < 4; j++)
                                {
                                    if (state1[j, i] < 16)
                                    {
                                        textBox8.Text += "0" + Convert.ToString(state1[j, i], 16);
                                        textBox9.Text += Convert.ToChar(state1[j, i]);
                                    }
                                    else
                                    {
                                        textBox8.Text += Convert.ToString(state1[j, i], 16);
                                        textBox9.Text += Convert.ToChar(state1[j, i]);
                                    }
                                }
                        }
                        //шифрование файла
                        else
                        {
                            byte[,] stateen = new byte[4, 4];
                            long kc = fs.Length/16;
                            long read = 0;
                            long write = 0;
                            //Reg(fs.Name,0);
                            FileStream fsen = new FileStream(Reg(fs.Name, 0), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            Thread tr = new Thread(() =>
                                {
                                    for (long l = 0; l < kc; l++)
                                    {
                                        for (int i = 0; i < 4; i++)
                                            for (int j = 0; j < 4; j++)
                                            { fs.Seek(read, SeekOrigin.Begin); state[j, i] = Convert.ToByte(fs.ReadByte()); k++; read = fs.Position; }
                                        AES st = new AES(state, key);
                                        stateen = st.Encrypt();
                                        for (int i = 0; i < 4; i++)
                                            for (int j = 0; j < 4; j++)
                                            { fsen.Seek(write, SeekOrigin.Begin); fsen.WriteByte(stateen[j, i]); write++; }
                                    }
                                    MessageBox.Show("Шифрование окончено!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    fs.Close();
                                    fs = null;
                                    fsen.Close();
                                    fsen = null;
                                });
                            textBox3.Text = "";
                            tr.Start();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
        }

        private string Reg(string str, int flag)
        {
            string reg,temp = null;
            temp = Regex.Replace(str, @"^.*\\", "");
            reg = temp;
            if (flag == 0)
            { reg = "Encrypted_" + reg; }
            else { reg = "Decrypted_" + reg; }
            temp = Regex.Replace(str, temp, reg);
            return temp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = "";
                for (int i = 0; i < textBox1.Text.Length; i++)
                    textBox2.Text += Convert.ToString(Convert.ToByte(textBox1.Text[i]), 16);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void TextChanged2(object sender, EventArgs e)
        {
            try
            {
                textBox5.Text = "";
                for (int i = 0; i < textBox6.Text.Length; i++)
                    textBox5.Text += Convert.ToString(Convert.ToByte(textBox6.Text[i]), 16);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox11.Text = "";
            byte[,] state = new byte[4, 4];
            int k = 0;
            int Nk = 0;
            if (comboBox1.SelectedIndex == 0) { Nk = 4; }
            if (comboBox1.SelectedIndex == 1) { Nk = 6; }
            if (comboBox1.SelectedIndex == 2) { Nk = 8; }
            byte[][] key = new byte[Nk][];
            for (int i = 0; i < Nk; i++)
            {
                key[i] = new byte[4];
            }
            if ((textBox17.Text == "" || textBox14.Text == "") && fs == null ) { MessageBox.Show("Заполните поля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                try
                {
                    for (int i = 0; i < Nk; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            string temp = Convert.ToString(textBox17.Text[k]) + Convert.ToString(textBox17.Text[k + 1]);
                            key[i][j] = Convert.ToByte(temp, 16);
                            k += 2;
                        }
                    }
                    k = 0;
                    if (fs == null)
                    {
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                            {
                                string temp = Convert.ToString(textBox14.Text[k]) + Convert.ToString(textBox14.Text[k + 1]);
                                state[j, i] = Convert.ToByte(temp, 16);
                                k += 2;
                            }
                        AES aes = new AES(state, key);
                        byte[,] state1 = aes.Decrypt();
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                            {
                                if (state1[j, i] < 16)
                                {
                                    textBox11.Text += "0" + Convert.ToString(state1[j, i], 16);
                                    textBox12.Text += Convert.ToChar(state1[j, i]);
                                }
                                else
                                {
                                    textBox11.Text += Convert.ToString(state1[j, i], 16);
                                    textBox12.Text += Convert.ToChar(state1[j, i]);
                                }
                            }
                    }
                    //Дешифрование файла
                    else
                    {
                        byte[,] stateen = new byte[4, 4];
                        long kc = fs.Length / 16;
                        long read = 0;
                        long write = 0;
                        FileStream fsen = new FileStream(Reg(fs.Name, 1), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        Thread tr = new Thread(() =>
                        {
                            for (long l = 0; l < kc; l++)
                            {
                                for (int i = 0; i < 4; i++)
                                    for (int j = 0; j < 4; j++)
                                    { fs.Seek(read, SeekOrigin.Begin); state[j, i] = Convert.ToByte(fs.ReadByte()); k++; read = fs.Position; }
                                AES st = new AES(state, key);
                                stateen = st.Decrypt();
                                for (int i = 0; i < 4; i++)
                                    for (int j = 0; j < 4; j++)
                                    { fsen.Seek(write, SeekOrigin.Begin); fsen.WriteByte(stateen[j, i]); write++; }
                            }
                            MessageBox.Show("Дешифрование окончено!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            fs.Close();
                            fsen.Close();
                            fsen = null;
                            fs = null;
                            
                        });
                        textBox3.Text = "";
                        tr.Start();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void TextChanged3(object sender, EventArgs e)
        {
            try
            {
                textBox17.Text = "";
                for (int i = 0; i < textBox18.Text.Length; i++)
                    textBox17.Text += Convert.ToString(Convert.ToByte(textBox18.Text[i]), 16);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void TextChanged4(object sender, EventArgs e)
        {
            try
            {
                textBox14.Text = "";
                for (int i = 0; i < textBox15.Text.Length; i++)
                    textBox14.Text += Convert.ToString(Convert.ToByte(textBox15.Text[i]), 16);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==0)
            {
            textBox1.MaxLength = 16;
            textBox2.MaxLength = 32;
            textBox6.MaxLength = 16;
            textBox5.MaxLength = 32;
            textBox17.MaxLength = 32;
            textBox18.MaxLength = 16;
            textBox14.MaxLength = 32;
            textBox15.MaxLength = 16;
            }
            if(comboBox1.SelectedIndex==1)
            {
            textBox1.MaxLength = 24;
            textBox2.MaxLength = 48;
            textBox6.MaxLength = 16;
            textBox5.MaxLength = 48;
            textBox17.MaxLength = 48;
            textBox18.MaxLength = 24;
            textBox14.MaxLength = 32;
            textBox15.MaxLength = 24;
            }
            if (comboBox1.SelectedIndex == 2)
            {
            textBox1.MaxLength = 32;
            textBox2.MaxLength = 64;
            textBox6.MaxLength = 16;
            textBox5.MaxLength = 32;
            textBox17.MaxLength = 64;
            textBox18.MaxLength = 32;
            textBox14.MaxLength = 64;
            textBox15.MaxLength = 16;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            {
                fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                textBox3.Text = openFileDialog1.FileName + "\r\n";
            }
            catch (Exception ee) { MessageBox.Show(ee.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fs.Close();
            fs = null;
            textBox3.Text = "";
        }
    }
}
