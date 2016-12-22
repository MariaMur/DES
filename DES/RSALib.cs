using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DES_1
{
    public class RSASign : ISign
    {
        public void Sign(Stream input, Stream output)
        {
            Stream hash = new MemoryStream();
            SHA1Lib sha = new SHA1Lib();
            sha.Hash(input, hash);
            RSALib rsa = new RSALib();
            rsa.EncryptData(hash, output);
        }
    }
    public class RSALib
    {
        char[] characters = new char[] { '#', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 
                                                'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', 
                                                '3', '4', '5', '6', '7', '8', '9', '0' };
        long p = 3;
        long q = 2;
        long n;
        long d;

        public void EncryptData(Stream input, Stream output)
        {
            if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
            {
                string s = "";
                byte[] bufRead = new byte[input.Length];
                byte[] bufWrite;/* = new byte[input.Length];*/
                input.Read(bufRead, 0, Convert.ToInt32(input.Length));
                s = Encoding.Default.GetString(bufRead);

                s = s.ToUpper();

                n = p * q;
                long m = (p - 1) * (q - 1);
                d = Calculate_d(m);
                long e_ = Calculate_e(d, m);

                List<string> result = RSA_Endoce(s, e_, n);

                foreach (string item in result)
                {
                    bufWrite = Encoding.Default.GetBytes(item);
                    output.Write(bufWrite, 0, Convert.ToInt32(bufWrite.Length));
                    output.WriteByte(Convert.ToByte('/'));
                }
                output.Position = 0;
            }
        }
        public void DecryptData(Stream input, Stream output)
        {
            List<string> inputL = new List<string>();
            int length = Convert.ToInt32(input.Length);
            string s;
            char[] s1 = new char[length];
            byte[] bufRead = new byte[length];
            byte[] bufWrite;/* = new byte[input.Length];*/
            input.Read(bufRead, 0, length);
            s = Encoding.Default.GetString(bufRead);
            s = s.ToUpper();
            int l = 0;
            for (int i = 0; s[i] != 0; i++)
            {
                if (s[i] == '/')
                {
                    inputL.Add(Convert.ToString(s1));
                    l = 0;
                }
                else
                {
                    s1[l] = s[i];
                    l++;
                }
            }

            string result = RSA_Dedoce(inputL, d, n);
            output.Write(Encoding.Default.GetBytes(result), Convert.ToInt32(output.Position), (Encoding.Default.GetBytes(result)).Length);


        }
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
        private List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            long bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = index;
                bi = (long)Math.Pow(bi, (int)e);

                long n_ = n;

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }
        private string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            long bi;

            foreach (string item in input)
            {
                bi = (long)Convert.ToDouble(item);
                bi = (long)Math.Pow(bi, (int)d);

                long n_ = n;

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }
        private long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        private long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
    }

}
