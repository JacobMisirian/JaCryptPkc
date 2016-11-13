using System;
using System.IO;
using System.Numerics;

namespace Pkc.Cryptography
{
    public class PublicKey
    {
        public static PublicKey FromFile(string filePath)
        {
            BinaryReader reader = new BinaryReader(new StreamReader(filePath).BaseStream);
            var key = BigInteger.Parse(reader.ReadString());
            var e = BigInteger.Parse(reader.ReadString());
            reader.Close();
            return new PublicKey(key, e);
        }

        public BigInteger Key { get; private set; }
        public BigInteger E { get; private set; }

        public PublicKey(BigInteger key, BigInteger e)
        {
            Key = key;
            E = e;
        }

        public void Save(string filePath)
        {
            BinaryWriter writer = new BinaryWriter(new StreamWriter(filePath).BaseStream);
            writer.Write(Key.ToString());
            writer.Write(E.ToString());
            writer.Flush();
            writer.Close();
        }
    }
}

