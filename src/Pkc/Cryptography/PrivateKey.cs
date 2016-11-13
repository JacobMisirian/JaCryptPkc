using System;
using System.IO;
using System.Numerics;

namespace Pkc.Cryptography
{
    public class PrivateKey
    {
        public static PrivateKey FromFile(string filePath)
        {
            BinaryReader reader = new BinaryReader(new StreamReader(filePath).BaseStream);
            BigInteger key = BigInteger.Parse(reader.ReadString());
            reader.Close();

            return new PrivateKey(key);
        }

        public BigInteger Key { get; private set; }

        public PrivateKey(BigInteger key)
        {
            Key = key;
        }

        public void Save(string filePath)
        {
            BinaryWriter writer = new BinaryWriter(new StreamWriter(filePath).BaseStream);
            writer.Write(Key.ToString());

            writer.Flush();
            writer.Close();
        }
    }
}

