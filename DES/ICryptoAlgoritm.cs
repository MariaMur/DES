using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DES_1
{
    public interface ICryptoAlgorithm
    {
        void EncryptData(Stream input, Stream output);
        void DecryptData(Stream input, Stream output);
    }

    public interface IHash
    {
        void Hash(Stream input, Stream output);
    }
    public interface ISign
    {
        void Sign(Stream input, Stream output);
    }
}
