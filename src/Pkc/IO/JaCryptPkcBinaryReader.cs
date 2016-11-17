using System;
using System.IO;
using System.Numerics;
using System.Text;

using Pkc.Cryptography;

namespace Pkc.IO
{
    public class JaCryptPkcBinaryReader
    {
        public Stream BaseStream { get; private set; }
        public PrivateKey PrivateKey { get; private set; }
        public PublicKey PublicKey { get; private set; }

        private BinaryReader reader;
        private JaCryptPkc crypto;

        public JaCryptPkcBinaryReader(Stream stream, PublicKey myPublicKey, PrivateKey myPrivateKey)
        {
            BaseStream = stream;
            PublicKey = myPublicKey;
            PrivateKey = myPrivateKey;
            reader = new BinaryReader(BaseStream);
            crypto = new JaCryptPkc();
        }

        public void Close()
        {
            BaseStream.Close();
            reader.Close();
        }

        public PublicKey ReadPublicKey()
        {
            var key = BigInteger.Parse(reader.ReadString());
            var E = BigInteger.Parse(reader.ReadString());

            return new PublicKey(key, E);
        }

        public byte[] ReadEncryptedBytes()
        {
            var length = reader.ReadInt32();
            return ReadEncryptedBytes(length);
        }
        public byte[] ReadEncryptedBytes(int count)
        {
            return crypto.Decrypt(reader.ReadBytes(count), PublicKey, PrivateKey);
        }

        public double ReadEncryptedDouble()
        {
            return BitConverter.ToDouble(ReadEncryptedBytes(), 0);
        }

        public short ReadEncryptedInt16()
        {
            return BitConverter.ToInt16(ReadEncryptedBytes(), 0);
        }
        public int ReadEncryptedInt32()
        {
            return BitConverter.ToInt32(ReadEncryptedBytes(), 0);
        }
        public long ReadEncryptedInt64()
        {
            return BitConverter.ToInt64(ReadEncryptedBytes(), 0);
        }

        public string ReadEncryptedString()
        {
            return ASCIIEncoding.ASCII.GetString(ReadEncryptedBytes());
        }

        public ushort ReadEncryptedUInt16()
        {
            return BitConverter.ToUInt16(ReadEncryptedBytes(), 0);
        }
        public uint ReadEncryptedUInt32()
        {
            return BitConverter.ToUInt32(ReadEncryptedBytes(), 0);
        }
        public ulong ReadEncryptedUInt64()
        {
            return BitConverter.ToUInt64(ReadEncryptedBytes(), 0);
        }
    }
}

