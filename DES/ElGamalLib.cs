using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DES_1
{

    public class ElGamalLib : ICryptoAlgorithm
    {
        public int p = 257;
        public int x = 77;
        public int g = 2; 
        public int y;

        public void DecryptData(Stream input, Stream output)
        {
            byte a;
            byte b;
            byte[] bytesToDecrypt = new byte[2];
            input.Position = 0;
            for (; input.Position < input.Length; )
            {
                input.Read(bytesToDecrypt, 0, 2);

                a = bytesToDecrypt[0];
                b = bytesToDecrypt[1];

                int deM = Multiple(b, Power(a, p - 1 - x, p), p);
                output.WriteByte((byte)deM);
            }
        }
        public void EncryptData(Stream input, Stream output)
        {
            byte byteToCrypt;

            y = Power(g, x, p);

            input.Position = 0;
            for (; input.Position < input.Length; )
            {
                byteToCrypt = (byte)input.ReadByte();

                int k = Rand() % (p - 2) + 1;
                int a = Power(g, k, p);
                int b = Multiple(Power(y, k, p), byteToCrypt, p);

                output.WriteByte((byte)a);
                output.WriteByte((byte)b);


            }
        }
        public void SetKey(string key)
        {
            string[] keys = key.Split(' ');
            try
            {
                p = Convert.ToInt32(keys[0]);
                g = Convert.ToInt32(keys[1]);
                x = Convert.ToInt32(keys[2]);
            }
            catch (Exception ex)
            { };

        }

        public int Rand()
        {
            Random random = new Random();
            return random.Next();
        }
        public int Power(int a, int b, int m) // a^b mod m
        {
            int tmp = a;
            int sum = tmp;
            for (int i = 1; i < b; i++)
            {
                for (int j = 1; j < a; j++)
                {
                    sum += tmp;
                    if (sum >= m)
                    {
                        sum -= m;
                    }
                }
                tmp = sum;
            }
            return tmp;
        }
        public int Multiple(int a, int b, int m) 
        {
            int sum = 0;
            for (int i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= m)
                {
                    sum -= m;
                }
            }
            return sum;
        }
    }
}
