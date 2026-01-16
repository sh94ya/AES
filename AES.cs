using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AES
{
    public class AES
    {
        //Переменные
        public byte[,] state = new byte[4, 4];
        private byte[][] key;
        public int Nr;
        private int Nk;
        public int Nb = 4;
        //private byte[][] W;
        public byte[][] W;
        public byte[,] temp = new byte[4, 4];
        public byte[] Sbox = {
        0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, 
        0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, 
        0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, 
        0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, 
        0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, 
        0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, 
        0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, 
        0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, 
        0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, 
        0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb, 
        0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, 
        0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08, 
        0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, 
        0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, 
        0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, 
        0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
        };
        byte[][] Rcon = new byte[10][];
        byte[] InvSbox = {
        0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
        0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
        0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
        0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
        0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
        0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
        0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
        0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
        0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
        0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
        0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
        0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
        0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
        0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
        0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
        0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
        };

        //Конструктор
        public AES(byte[,] state, byte[][] key)
        {
            this.state = state;
            this.key = key;
            if (key.Length == 4) { Nr = 10; Nk = 4; }
            if (key.Length == 6) { Nr = 12; Nk = 6; }
            if (key.Length == 8) { Nr = 14; Nk = 8; }
            W = new byte[4 * (Nr + 1)][];
            for (int i = 0; i < 4 * (Nr + 1); i++)
            {
                W[i] = new byte[4];
            }
            for (int i = 0; i < 10; i++)
            {
                Rcon[i] = new byte[4];
            }
            Rcon[0][0] = 0x01; Rcon[1][0] = 0x02; Rcon[2][0] = 0x04; Rcon[3][0] = 0x08; Rcon[4][0] = 0x10;
            Rcon[5][0] = 0x20; Rcon[6][0] = 0x40; Rcon[7][0] = 0x80; Rcon[8][0] = 0x1b; Rcon[9][0] = 0x36;
        }

        #region Блоки Шифрования
        public void ExpandKey()
        {
            if (Nk <= 6)
            {
                for (int i = 0; i < Nk; i++)
                    W[i] = key[i];

                for (int j = Nk; j < 2 * Nb * (Nk + 1) + 4; j += Nk)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (j - 1 < Nb * (Nk + 1))
                        {
                            byte[] temp = SubWord(RotWord(j - 1));
                            W[j][k] = Convert.ToByte((W[j - Nk][k]) ^ (temp[k]) ^ (Rcon[j / Nk - 1][k]));
                        }
                    }
                    for (int i = 1; i < Nk && i + j < Nb * (Nr + 1); i++)
                        for (int k = 0; k < 4; k++)
                            W[i + j][k] = Convert.ToByte((W[i + j - Nk][k]) ^ (W[i + j - 1][k]));
                }
            }
            else
            {
                for (int i = 0; i < Nk; i++)
                    W[i] = key[i];

                for (int j = Nk; j < 2 * Nb * (Nk + 1); j += Nk)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (j - 1 < W.Length)
                        {
                            byte[] temp = SubWord(RotWord(j - 1));
                            W[j][k] = Convert.ToByte(W[j - Nk][k] ^ temp[k] ^ Rcon[j / Nk - 1][k]);
                        }
                    }
                    for (int i = 1; i < 4; i++)
                        for (int k = 0; k < 4; k++)
                        {
                            if (i + j < W.Length)
                                W[i + j][k] = Convert.ToByte(W[i + j - Nk][k] ^ W[i + j - 1][k]);
                        }
                    for (int k = 0; k < 4; k++)
                    {
                        if (j + 4 < W.Length)
                        {
                            byte[] temp = SubWord(W[j + 3]);
                            W[j + 4][k] = Convert.ToByte(W[j + 4 - Nk][k] ^ temp[k]);
                        }
                    }
                    for (int i = 5; i < Nk; i++)
                        for (int k = 0; k < 4; k++)
                            if (i + j < W.Length)
                                W[i + j][k] = Convert.ToByte(W[i + j - Nk][k] ^ W[i + j - 1][k]);
                }
            }
        }
        public void SubBites()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i, j] = Sbox[state[i, j]];
                }
            }
        }
        public void ShiftRows()
        {
            byte temp;
            //сдвиг первой строки на 1
            temp = state[1, 0];
            state[1, 0] = state[1, 1];
            state[1, 1] = state[1, 2];
            state[1, 2] = state[1, 3];
            state[1, 3] = temp;
            //сдвиг второй строки на 2
            temp = state[2, 0];
            state[2, 0] = state[2, 2];
            state[2, 2] = temp;
            temp = state[2, 1];
            state[2, 1] = state[2, 3];
            state[2, 3] = temp;
            //сдвиг третьей строки на 3
            temp = state[3, 0];
            state[3, 0] = state[3, 3];
            state[3, 3] = state[3, 2];
            state[3, 2] = state[3, 1];
            state[3, 1] = temp;
        }
        public void MixColumns()
        {
            byte[] temp;
            for (int j = 0; j < 4; j++)
            {
                temp = new byte[4];
                temp[0] = Convert.ToByte(mul_by_02(state[0, j]) ^ mul_by_03(state[1, j]) ^ state[2, j] ^ state[3, j]);
                temp[1] = Convert.ToByte(state[0, j] ^ mul_by_02(state[1, j]) ^ mul_by_03(state[2, j]) ^ state[3, j]);
                temp[2] = Convert.ToByte(state[0, j] ^ state[1, j] ^ mul_by_02(state[2, j]) ^ mul_by_03(state[3, j]));
                temp[3] = Convert.ToByte(mul_by_03(state[0, j]) ^ state[1, j] ^ state[2, j] ^ mul_by_02(state[3, j]));
                for (int i = 0; i < 4; i++)
                {
                    state[i, j] = temp[i];
                }
            }
        }

        public void AddRoundKey(int index)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i, j] = Convert.ToByte(Convert.ToInt32(state[i, j]) ^ Convert.ToInt32(W[(index * Nb) + i][j]));
                }
            }
        }
        #endregion
        #region Вспомогательные функции
        protected int mul_by_02(int num)
        {
            int res;
            if (num < 0x80)
            { res = (num << 1); }
            else { res = (num << 1) ^ 0x1b; }

            return (res % 0x100);

        }

        protected int mul_by_03(int num)
        {
            return mul_by_02(num) ^ num;
        }

        protected int mul_by_09(int num)
        {
            return mul_by_02(mul_by_02(mul_by_02(num))) ^ num;
        }

        protected int mul_by_0b(int num)
        {
            return mul_by_02(mul_by_02(mul_by_02(num))) ^ mul_by_02(num) ^ num;
        }

        protected int mul_by_0d(int num)
        {
            return mul_by_02(mul_by_02(mul_by_02(num))) ^ mul_by_02(mul_by_02(num)) ^ num;
        }

        protected int mul_by_0e(int num)
        {
            return mul_by_02(mul_by_02(mul_by_02(num))) ^ mul_by_02(mul_by_02(num)) ^ mul_by_02(num);
        }
        protected byte[] SubWord(byte[] Sw)
        {
            // byte[] temp = Sw;
            byte[] temp = new byte[Sw.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = Sw[i];
            }
            for (int i = 0; i < 4; i++)
            {
                temp[i] = Sbox[temp[i]];
            }
            return temp;
        }
        protected byte[] RotWord(int index)
        {
            byte[] tempMas = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tempMas[i] = W[index][i];
            }
            byte temp = tempMas[0];
            tempMas[0] = tempMas[1];
            tempMas[1] = tempMas[2];
            tempMas[2] = tempMas[3];
            tempMas[3] = temp;
            return tempMas;
        }
        protected void Reverse()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    temp[i, j] = state[j, i];
            //for (int i = 0; i < 4; i++)
            //    for (int j = 0; j < 4; j++)
            // state[i, j] = temp[j, i];
        }

        #endregion
        #region Основные Методы
        public byte[,] Encrypt()
        {
            ExpandKey();
            AddRoundKey(0);
            //Reverse();
            for (int i = 1; i < Nr; i++)
            {
                SubBites();
                //Reverse();
                ShiftRows();
                //Reverse();
                MixColumns();
                //Reverse();
                AddRoundKey(i);
                //Reverse();
            }
            SubBites();
            ShiftRows();
            //Reverse();
            AddRoundKey(Nr);
            //Reverse();
            return state;
        }
        public byte[,] Decrypt()
        {

            ExpandKey();
            AddRoundKey(Nr);
            for (int i = Nr - 1; i >= 1; i--)
            {
                InvShiftRows();
                InvSubBites();
                AddRoundKey(i);
                InvMixColumns();
            }
            InvShiftRows();
            InvSubBites();
            AddRoundKey(0);
            return state;
        }
        #endregion
        #region Блоки дешифрования
        private void InvSubBites()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    state[i, j] = InvSbox[state[i, j]];
                }
            }
        }
        protected void InvShiftRows()
        {
            byte temp;
            //сдвиг первой строки на 1
            temp = state[1, 3];
            state[1, 3] = state[1, 2];
            state[1, 2] = state[1, 1];
            state[1, 1] = state[1, 0];
            state[1, 0] = temp;
            //сдвиг второй строки на 2
            temp = state[2, 3];
            state[2, 3] = state[2, 1];
            state[2, 1] = temp;
            temp = state[2, 2];
            state[2, 2] = state[2, 0];
            state[2, 0] = temp;
            //сдвиг третьей строки на 3
            temp = state[3, 3];
            state[3, 3] = state[3, 0];
            state[3, 0] = state[3, 1];
            state[3, 1] = state[3, 2];
            state[3, 2] = temp;
        }
        protected void InvMixColumns()
        {
            byte[] temp;
            for (int j = 0; j < 4; j++)
            {
                temp = new byte[4];
                temp[0] = Convert.ToByte(mul_by_0e(state[0, j]) ^ mul_by_0b(state[1, j]) ^ mul_by_0d(state[2, j]) ^ mul_by_09(state[3, j]));
                temp[1] = Convert.ToByte(mul_by_09(state[0, j]) ^ mul_by_0e(state[1, j]) ^ mul_by_0b(state[2, j]) ^ mul_by_0d(state[3, j]));
                temp[2] = Convert.ToByte(mul_by_0d(state[0, j]) ^ mul_by_09(state[1, j]) ^ mul_by_0e(state[2, j]) ^ mul_by_0b(state[3, j]));
                temp[3] = Convert.ToByte(mul_by_0b(state[0, j]) ^ mul_by_0d(state[1, j]) ^ mul_by_09(state[2, j]) ^ mul_by_0e(state[3, j]));
                for (int i = 0; i < 4; i++)
                {
                    state[i, j] = temp[i];
                }
            }
        }
        #endregion
    }
}
