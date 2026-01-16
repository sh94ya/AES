using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AES
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.MaxLength = 16;
            textBox2.MaxLength = 32;
            textBox3.MaxLength = 16;
            textBox4.MaxLength = 32;
            comboBox1.Items.Add("128");
            comboBox1.SelectedIndex = 0;
            comboBox1.Items.Add("192");
            comboBox1.Items.Add("256");
            textBox5.ScrollBars = ScrollBars.Vertical;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[,] state = new byte[4, 4];
            int k = 0;
            int Nk = 0;
            int count = 0;
            if (comboBox1.SelectedIndex == 0) { Nk = 4; }
            if (comboBox1.SelectedIndex == 1) { Nk = 6; }
            if (comboBox1.SelectedIndex == 2) { Nk = 8; }
            byte[][] key = new byte[Nk][];
            for (int i = 0; i < Nk; i++)
            {
                key[i] = new byte[4];
            }
            if (textBox2.Text == "" || textBox4.Text == "") { MessageBox.Show("Заполните поля", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                try
                {
                    for (int i = 0; i < Nk; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            string temp = Convert.ToString(textBox4.Text[k]) + Convert.ToString(textBox4.Text[k + 1]);
                            key[i][j] = Convert.ToByte(temp, 16);
                            k += 2;
                        }
                    }
                    k = 0;
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                        {
                            string temp = Convert.ToString(textBox2.Text[k]) + Convert.ToString(textBox2.Text[k + 1]);
                            state[j, i] = Convert.ToByte(temp, 16);
                            k += 2;
                        }
                    AES aes = new AES(state, key);
                    aes.ExpandKey();
                    aes.AddRoundKey(0);
                    textBox5.Text = "Раунд 0:     Ключ раунда: ";
                    printkey(aes,0);
                    textBox5.Text += "add_roundkey: ";
                    print(aes);
                     for (int i = 1; i < aes.Nr; i++)
                        {
                            count = i;
                            textBox5.Text += "Раунд" + Convert.ToString(i) + ":     Ключ раунда: ";
                            printkey(aes, i);
                            aes.SubBites();
                            textBox5.Text += "sub_bytes: ";
                            print(aes);
                            aes.ShiftRows();
                            textBox5.Text += "shift_rows: ";
                            print(aes);
                            aes.MixColumns();
                            textBox5.Text += "mix_columns: ";
                            print(aes);
                            aes.AddRoundKey(i);
                            textBox5.Text += "add_roundkey: ";
                            print(aes);
                        }
                     textBox5.Text += "Раунд " + Convert.ToString(count + 1) + ":     Ключ раунда: ";
                     printkey(aes, count + 1);
                     aes.SubBites();
                     textBox5.Text += "sub_bytes: ";
                     print(aes);
                     aes.ShiftRows();
                     textBox5.Text += "shift_rows: ";
                     print(aes);
                     aes.AddRoundKey(aes.Nr);
                     textBox5.Text += "add_roundkey: ";
                     print(aes);
     
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }
        public void print(AES aes)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (aes.state[j, i]<16)
                    { textBox5.Text += "0" + Convert.ToString(aes.state[j, i], 16); }
                    else
                    {
                        textBox5.Text += Convert.ToString(aes.state[j, i], 16);
                    }
            textBox5.Text += "\r\n";
        }

        public void printkey(AES aes,int index)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (aes.W[i, (index * aes.Nb) + j] < 16)
                    {
                        textBox5.Text += "0" + Convert.ToString(aes.W[i, (index * aes.Nb) + j], 16);
                    }
                    else 
                    { textBox5.Text += Convert.ToString(aes.W[i,(index * aes.Nb) + j], 16); }
            textBox5.Text += "\r\n";
        }

        private void Key1Changed(object sender, EventArgs e)
        {
            try
            {
                textBox4.Text = "";
                for (int i = 0; i < textBox3.Text.Length; i++)
                    textBox4.Text += Convert.ToString(Convert.ToByte(textBox3.Text[i]), 16);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox3.MaxLength = 16;
                textBox4.MaxLength = 32;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox3.MaxLength = 24;
                textBox4.MaxLength = 48;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                textBox3.MaxLength = 32;
                textBox4.MaxLength = 64;
            }
        }
    }
}