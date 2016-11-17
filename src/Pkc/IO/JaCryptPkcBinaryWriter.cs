using System;
using System.IO;
using System.Numerics;
using System.Text;

using Pkc.Cryptography;

namespace Pkc.IO
{
    public class JaCryptPkcBinaryWriter
    {
        public Stream BaseStream { get; private set; }
        public PrivateKey PrivateKey { get; private set; }
        public PublicKey PublicKey { get; private set; }

        public int KeyLength { get; set; }

        private BinaryWriter writer;
        private JaCryptPkc crypto;

        public JaCryptPkcBinaryWriter(Stream stream, PublicKey theirPublicKey, int keyLength = 128)
        {
            BaseStream = stream;
            PublicKey = theirPublicKey;
            writer = new BinaryWriter(BaseStream);
            crypto = new JaCryptPkc();
            KeyLength = keyLength;
        }

        public void Close()
        {
            BaseStream.Close();
            writer.Close();
        }

        public void Flush()
        {
            writer.Flush();
        }

        public void WritePublicKey(PublicKey publicKey)
        {
            writer.Write(publicKey.Key.ToString());
            writer.Write(publicKey.E.ToString());
        }

        public void WriteEncrypted(byte[] bytes)
        {
            bytes = crypto.Encrypt(bytes, PublicKey, KeyLength);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        public void WriteEncrypted(double d)
        {
            WriteEncrypted(BitConverter.GetBytes(d));
        }

        public void WriteEncrypted(short s)
        {
            WriteEncrypted(BitConverter.GetBytes(s));
        }
        public void WriteEncrypted(int i)
        {
            WriteEncrypted(BitConverter.GetBytes(i));
        }
        public void WriteEncrypted(long l)
        {
            WriteEncrypted(BitConverter.GetBytes(l));
        }

        public void WriteEncrypted(string s)
        {
            WriteEncrypted(ASCIIEncoding.ASCII.GetBytes(s));
        }

        public void WriteEncrypted(ushort s)
        {
            WriteEncrypted(BitConverter.GetBytes(s));
        }
        public void WriteEncrypted(uint i)
        {
            WriteEncrypted(BitConverter.GetBytes(i));
        }
        public void WriteEncrypted(ulong l)
        {
            WriteEncrypted(BitConverter.GetBytes(l));
        }
    }
}

