using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DES_1;

namespace DESTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void DESTestSourceDecrypt()
        {
            string source = "АБВГ";
            string encrypt;
            DESLib crypt = new DESLib();
            Stream sourceData = new MemoryStream(Encoding.Default.GetBytes(source));
            Stream encryptedData = new MemoryStream();
            crypt.EncryptData(sourceData, encryptedData); 
            Stream decryptedData = new MemoryStream();
            crypt.DecryptData(encryptedData, decryptedData);
           
            byte[] bufRead1 = new byte[sourceData.Length];
            sourceData.Read(bufRead1, 0, Convert.ToInt32(sourceData.Length));
            byte[] bufRead2 = new byte[sourceData.Length];
            sourceData.Read(bufRead2, 0, Convert.ToInt32(sourceData.Length));
            source = Encoding.Default.GetString(bufRead1);
            encrypt = Encoding.Default.GetString(bufRead2);
            
            Assert.AreEqual(source, encrypt);

        }
        [TestMethod]
        public void DESTestDecrypt()
        {                        
            string source = "АБВГДЕЁЖ";
            string encrypt;

            DESLib crypt = new DESLib();

            Stream sourceData = new MemoryStream(Encoding.Default.GetBytes(source));
            Stream encryptedData = new MemoryStream();
            crypt.EncryptData(sourceData, encryptedData);
            byte[] bufRead1 = new byte[encryptedData.Length];
           
            encryptedData.Read(bufRead1, 0, Convert.ToInt32(encryptedData.Length));
            encrypt = BitConverter.ToString(bufRead1);//.UTF8.GetString(bufRead1);
            Assert.AreEqual(encrypt, "00-00-00-01-00-01-00-00-01-01-00-00-00-01-00-00-01-00-01-00-01-00-01-00-01-00-00-00-01-01-00-00-01-01-00-01-01-01-00-00-00-01-01-01-01-01-00-00-00-01-00-00-00-00-00-00-00-01-01-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00"); 

        }
        [TestMethod]
        public void ElGamalTestSourceDecrypt()
        {
            string sourceDataString = "АБВГ";
            string decryptedDataString;

            ElGamalLib crypt = new ElGamalLib();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            crypt.EncryptData(sourceDataStream, encryptedDataStream);
            crypt.DecryptData(encryptedDataStream, decryptedDataStream);

            decryptedDataString = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDataString, decryptedDataString);
        

        }
        [TestMethod]
        public void ElGamalTestEncryptDecryptLenght()
        {
            string sourceDataString1 = "КРИПТОГРАФИЯ";
            string sourceDataString2 = "А";
            string sourceDataString3 = "АЛГОРИТМ ШИФРОВАНИЯ ELGAMAL";
            string decryptedDataString;
            string encrypt;

            ElGamalLib crypt = new ElGamalLib();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString1));
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            crypt.EncryptData(sourceDataStream, encryptedDataStream);
            crypt.DecryptData(encryptedDataStream, decryptedDataStream);

            decryptedDataString = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDataString1, decryptedDataString);

            sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString2));
            encryptedDataStream = new MemoryStream();
            decryptedDataStream = new MemoryStream();

            crypt.EncryptData(sourceDataStream, encryptedDataStream);
            crypt.DecryptData(encryptedDataStream, decryptedDataStream);

            decryptedDataString = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDataString2, decryptedDataString);

            sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString3));
            encryptedDataStream = new MemoryStream();
            decryptedDataStream = new MemoryStream();

            crypt.EncryptData(sourceDataStream, encryptedDataStream);
            crypt.DecryptData(encryptedDataStream, decryptedDataStream);

            decryptedDataString = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDataString3, decryptedDataString);

        }
        [TestMethod]
        public void SHA1TestHash()
        {
            string sourceDataString = "АБВГ";
            string hashDataString;

            SHA1Lib sha = new SHA1Lib();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream hashDataStream = new MemoryStream();

            sha.Hash(sourceDataStream, hashDataStream);

            hashDataString = Encoding.Default.GetString(hashDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual("f62a192ef62a192ef62a192ef62a192ef62a192e", hashDataString);


        }
        [TestMethod]
        public void RSASign()
        {
            string sourceDataString = "АБВГ";
            string signDataString;
            string sign2DataString;

            RSASign sign = new RSASign();
            RSALib rsa = new RSALib();
            SHA1Lib sha = new SHA1Lib();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream signDataStream = new MemoryStream();
            MemoryStream sign2DataStream = new MemoryStream();
            MemoryStream hashDataStream = new MemoryStream();

            sha.Hash(sourceDataStream, hashDataStream);
            sourceDataStream.Position = 0;
            hashDataStream.Position = 0;
            rsa.EncryptData(hashDataStream, sign2DataStream);

            sign.Sign(sourceDataStream, signDataStream);

            signDataString = Encoding.Default.GetString(signDataStream.ToArray()).TrimEnd('\0');
            sign2DataString = Encoding.Default.GetString(sign2DataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(signDataString, sign2DataString);


        }
                
    }
}
