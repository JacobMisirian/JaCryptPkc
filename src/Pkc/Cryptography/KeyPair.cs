using System;
using System.Numerics;

namespace Pkc.Cryptography
{
    public class KeyPair
    {
        public PublicKey PublicKey { get; private set; }
        public PrivateKey PrivateKey { get; private set; }

        public KeyPair(BigInteger publicKey, BigInteger e, BigInteger privateKey)
        {
            PublicKey = new PublicKey(publicKey, e);
            PrivateKey = new PrivateKey(privateKey);
        }
    }
}

