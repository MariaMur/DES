using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DES_1
{
   
    public class DESLib : ICryptoAlgorithm
    {
        #region Variables

        int[,] key;
        int[] e = new int[]{ 32,1,2,3,4,5,4,5,6,7,8,9,8,9,10,11,12,13,
                            12,13,14,15,16,17,16,17,18,19,20,21,20,21,22,23,24,25,
                            24,25,26,27,28,29,28,29,30,31,32,1 };
        int[] ip = new int[] { 58,50,42,34,26,18,10,2,60,52,44,36,28,20,12,4,
                            62,54,46,38,30,22,14,6,64,56,48,40,32,24,16,8,
                            57,49,41,33,25,17,9,1,59,51,43,35,27,19,11,3,
                            61,53,45,37,29,21,13,5,63,55,47,39,31,23,15,7 };
        int[] sm = new int[] {
                    14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7,
                    0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8,
                    4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0,
                    15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13,
                    15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10,
                    3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5,
                    0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15,
                    13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9,
                    10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8,
                    13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1,
                    13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7,
                    1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12,
                    7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15,
                    13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9,
                    10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4,
                    3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14,
                    2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9,
                    14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6,
                    4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14,
                    11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3,
                    12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11,
                    10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8,
                    9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6,
                    4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13,
                    4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1,
                    13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6,
                    1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2,
                    6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12,
                    13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7,
                    1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2,
                    7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8,
                    2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 };
        int[] p = new int[] { 16,7,20,21,29,12,28,17,
                        1,15,23,26,5,18,31,10,
                        2,8,24,14,32,27,3,9,
                        19,13,30,6,22,11,4,25};
        int[] _ip = new int[] { 40,8,48,16,56,24,64,32,39,7,47,15,55,23,63,31,
                                    38,6,46,14,54,22,62,30,37,5,45,13,53,21,61,29,
                                    36,4,44,12,52,20,60,28,35,3,43,11,51,19,59,27,
                                    34,2,42,10,50,18,58,26,33,1,41,9,49,17,57,25};
        #endregion
        public void EncryptData(Stream input, Stream output)
        {
            byte[] bufRead = new byte[input.Length];
            byte[] bufWrite = new byte[input.Length * 16];
            int[] write;
            input.Read(bufRead, 0, Convert.ToInt32(input.Length));

            String str = Encoding.Default.GetString(bufRead, 0, bufRead.Length);//BitConverter.ToString(bufRead).ToString(); 
            key = keys();
            int ln = 0;
            int step = 8;
            while (ln < str.Length)
            {
                if (str.Length - ln < step)
                    step = str.Length - ln;
                write = Encrypt(str.Substring(ln, step), key);
                for (int i = 0; i < write.Length; i++)
                    bufWrite[i] = Convert.ToByte(write[i]);
                output.Write(bufWrite, Convert.ToInt32(output.Position), Convert.ToInt32(bufWrite.Length));

                ln += step;
            }
            if (output.Position != 0)
                output.Position = 0;
        }
        public void DecryptData(Stream input, Stream output)
        {
            byte[] bufRead = new byte[input.Length];
            byte[] bufWrite = new byte[input.Length / 16];
            int[] read = new int[bufRead.Length];

            input.Read(bufRead, 0, Convert.ToInt32(input.Length));
            for (int i = 0; i < bufRead.Length; i++)
                read[i] = Convert.ToInt32(bufRead[i]);
            for (int i = 0; i < input.Length / 64; i++)
            {
                output.Write(Decrypt(read, key), Convert.ToInt32(output.Position), Convert.ToInt32(bufWrite.Length));
            }


        }
        private int[] Encrypt(string str, int[,] key)
        {
            byte[] buf = Encoding.GetEncoding(1251).GetBytes(str);

            int[] bitMass = new int[64];
            int[] t_ip = new int[64];
            int[][] l = new int[17][];
            int[][] r = new int[17][];
            for (int i = 0; i < 17; i++)
            {
                l[i] = new int[32];
                r[i] = new int[32];
            }
            List<int> list = new List<int>();
            for (int j = buf.Length - 1; j > -1; j--)
                for (int i = 0; i < 8; ++i)
                    list.Add((buf[j] >> i) & 1);

            list.Reverse();
            int t = 0;
            foreach (var item in list)
            {
                bitMass[t] = item;
                t++;
            }

            //Проверка на четность
            bool error = CheckKey(key);
            if (error)
            {
                return null;
            }

            //IP
            for (int i = 0; i < ip.Length; i++)
                t_ip[i] = bitMass[ip[i] - 1];

            //L[0]R[0]
            for (int i = 0; i < t_ip.Length; i++)
            {
                if (i < 32) l[0][i] = t_ip[i];
                else r[0][i - 32] = t_ip[i];
            }


            int[] ff_f = new int[32];
            int[] keys_t = new int[key.GetLength(1)];

            //16
            for (int i = 1; i < l.Length; i++)
            {
                l[i] = r[i - 1];
                for (int j = 0; j < keys_t.Length; j++)
                    keys_t[j] = key[i - 1, j];

                ff_f = ff(r[i - 1], keys_t);
                for (int j = 0; j < ff_f.Length; j++)
                    r[i][j] = l[i - 1][j] ^ ff_f[j];
            }

            int[] liri = new int[64];
            for (int i = 0; i < 64; i++)
            {
                if (i < 32) liri[i] = l[l.Length - 1][i];
                else liri[i] = r[r.Length - 1][i - 32];
            }

            int[] _out = new int[64];
            for (int i = 0; i < 64; i++)
                _out[i] = liri[_ip[i] - 1];

            return _out;
        }

        private byte[] Decrypt(int[] arr, int[,] key)
        {
            int[] bitMass = new int[64];
            int[] t_ip = new int[64];
            int[] liri = new int[64];
            int[] ff_f = new int[32];
            int[][] l = new int[17][];
            int[][] r = new int[17][];
            int[] keys_t = new int[key.GetLength(1)];
            List<int> list = new List<int>();

            bool error = CheckKey(key);
            if (error)
                return null;

            for (int i = 0; i < 17; i++)
            {
                l[i] = new int[32];
                r[i] = new int[32];
            }

            for (int i = 0; i < 64; i++)
                liri[_ip[i] - 1] = arr[i];

            for (int i = 0; i < 64; i++)
            {
                if (i < 32) l[l.Length - 1][i] = liri[i];
                else r[r.Length - 1][i - 32] = liri[i];
            }


            for (int i = l.Length - 1; i > 0; i--)
            {
                r[i - 1] = l[i];

                for (int j = 0; j < keys_t.Length; j++)
                    keys_t[j] = key[i - 1, j];
                ff_f = ff(r[i - 1], keys_t);
                for (int j = 0; j < ff_f.Length; j++)
                    l[i - 1][j] = r[i][j] ^ ff_f[j];
            }

            for (int i = 0; i < t_ip.Length; i++)
            {
                if (i < 32) t_ip[i] = l[0][i];
                else t_ip[i] = r[0][i - 32];
            }
            for (int i = 0; i < ip.Length; i++)
                bitMass[ip[i] - 1] = t_ip[i];


            for (int i = 0; i < bitMass.Length - 16; i++)
            {
                list.Add(bitMass[i]);
            }

            byte[] bytes = new byte[list.Count / 8];

            for (int i = 0; i < bytes.Length; i++)
            {
                StringBuilder t = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    t.Append(list[i * 8 + j].ToString());
                }
                bytes[i] = Convert.ToByte(Convert.ToInt32(t.ToString(), 2));
            }

            //var str = System.Text.Encoding.GetEncoding(1251).GetString(bytes);
            return bytes;//str;
        }
        private int[] ff(int[] mass, int[] key)
        {
            int[] fe = new int[e.Length];
            int[] b0 = new int[e.Length];
            int[,] b1 = new int[8, 6];
            int[] b2 = new int[8];
            String b3 = "";
            int[] b4;
            int[] bp = new int[32];

            for (int i = 0; i < e.Length; i++)
                fe[i] = mass[e[i] - 1];

            for (int i = 0; i < e.Length; i++)
                b0[i] = fe[i] ^ key[i];

            int iStr = 0, iStb = 0;
            for (int i = 0; i < e.Length; i++, iStb++)
            {
                if (iStb == 6)
                {
                    iStr++;
                    iStb = 0;
                }
                b1[iStr, iStb] = b0[i];
            }

            int[] lines = new int[8];
            int[] columen = new int[8];
            for (int i = 0; i < b1.GetLength(0); i++)
                for (int j = 0, t = 1, tt = 0; j < b1.GetLength(1); j++, t--, tt--)
                {
                    if (j < 2) { lines[i] += b1[i, j] * (int)Math.Pow(2, t); tt = 4; }
                    else columen[i] += b1[i, j] * (int)Math.Pow(2, tt);
                }

            for (int i = 0; i < b2.GetLength(0); i++)
            {
                b2[i] = sm[16 * (4 * i + lines[i]) + columen[i]];
                b3 += Convert.ToString(b2[i], 2).PadLeft(4, '0');
            }
            b4 = b3.Select(ch => int.Parse(ch.ToString())).ToArray();

            for (int i = 0; i < bp.Length; i++)
                bp[i] = b4[p[i] - 1];
            return bp;
        }

        private int[,] keys()
        {
            string str = "keytxt7";

            byte[] unicodeBytes = Encoding.Unicode.GetBytes(str);
            byte[] asciiBytes = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, unicodeBytes);
            int[] asciiInt = new int[asciiBytes.Length];
            int[] bitMass = new int[64];
            int[] pc = { 57,49,41,33,25,17,9,1,58,50,42,34,26,18,10,2,59,51,43,35,27,19,11,3,60,52,44,36,63,55,47,39,31,23,15,
                               7,62,54,46,38,30,22,14,6,61,53,45,37,29,21,13,5,28,20,12,4};
            int[] pc1 = { 14,17,11,24,1,5,3,28,15,6,21,10,23,19,12,4,26,8,16,7,27,20,13,2,41,52,31,37,47,55,30,40,51,45,33,48,
                              44,49,39,56,34,53,46,42,50,36,29,32 };
            int[] bitMass2 = new int[pc.Length];
            int[] c0 = new int[pc.Length / 2];
            int[] d0 = new int[pc.Length / 2];
            int[] shift = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            int[,] c = new int[shift.Length, pc.Length / 2];
            int[,] d = new int[shift.Length, pc.Length / 2];
            int[,] cd = new int[shift.Length, pc.Length];
            int[,] key = new int[shift.Length, pc1.Length];

            List<int> list = new List<int>();
            for (int j = asciiBytes.Length - 1; j > -1; j--)
                for (int i = 0; i < 8; ++i)
                    list.Add((asciiBytes[j] >> i) & 1);
            list.Reverse();
            int t = 0;
            foreach (var item in list)
            {
                bitMass[t] = item;
                t++;
            }

            for (int i = 0; i < pc.Length; i++)
                bitMass2[i] = bitMass[pc[i] - 1];

            for (int i = 0; i < bitMass2.Length; i++)
            {
                if (i < 28) c0[i] = bitMass2[i];
                else d0[i - 28] = bitMass2[i];
            }

            for (int tt = 0; tt < shift.Length; tt++)
            {
                for (int j = 0; j < shift[tt]; ++j)
                {
                    int temp = c0[0];
                    for (int i = 0; i < c0.Length - 1; i++)
                        c0[i] = c0[i + 1];
                    c0[c0.Length - 1] = temp;
                }
                for (int r = 0; r < pc.Length / 2; r++)
                    c[tt, r] = c0[r];
            }

            for (int tt = 0; tt < shift.Length; tt++)
            {
                for (int j = 0; j < shift[tt]; ++j)
                {
                    int temp = d0[0];
                    for (int i = 0; i < d0.Length - 1; i++)
                        d0[i] = d0[i + 1];
                    d0[d0.Length - 1] = temp;
                }
                for (int r = 0; r < pc.Length / 2; r++)
                    d[tt, r] = d0[r];
            }

            for (int i = 0; i < shift.Length; i++)
                for (int j = 0; j < pc.Length; j++)
                    cd[i, j] = (j > 27) ? d[i, j - 28] : c[i, j];

            for (int i = 0; i < shift.Length; i++)
                for (int j = 0; j < pc1.Length; j++)
                    key[i, j] = cd[i, pc1[j] - 1];


            return key;
        }

        private bool CheckKey(int[,] key)
        {
            for (int z = 0; z < key.GetLength(0); z++)
            {
                int sum = 0;
                for (int j = 0; j < key.GetLength(1); j++)
                {
                    sum += key[z, j];
                    if (j == key.GetLength(1) - 1)
                    {
                        if (key[z, j] == sum % 2)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
