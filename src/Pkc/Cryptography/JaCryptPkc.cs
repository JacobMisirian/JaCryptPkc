using System;
using System.Collections.Generic;
using System.Numerics;

using JaCrypt.Cryptography;

namespace Pkc.Cryptography
{
    public class JaCryptPkc
    {
        public void Test(PublicKey publicKey, PrivateKey privateKey)
        {

            BigInteger l = new BigInteger(generateRandomBigInteger());
            if (l < 0)
                l *= -1;
            Console.WriteLine("Real number {0}", l);
            BigInteger encryptedL = encryptKey(l, publicKey.Key, publicKey.E);
            Console.WriteLine("Encrypted number {0}", encryptedL);
            BigInteger decryptedL = decryptKey(encryptedL, publicKey.Key, privateKey.Key);
            Console.WriteLine("Decrypted number {0}", decryptedL);
        }


        public byte[] Encrypt(byte[] message, PublicKey publicKey)
        {
            List<byte> result = new List<byte>();
            byte[] key = generateRandomBigInteger();
            byte[] encryptedKey = encryptKey(new BigInteger(key), publicKey.Key, publicKey.E).ToByteArray();
            byte[] encryptedMsg = new JaCrypto().Encrypt(key, message);
            foreach (byte b in BitConverter.GetBytes(encryptedKey.Length))
                result.Add(b);
            foreach (byte b in encryptedKey)
                result.Add(b);
            foreach (byte b in encryptedMsg)
                result.Add(b);

            return result.ToArray();
        }

        public byte[] Decrypt(byte[] message, PublicKey publicKey, PrivateKey privateKey)
        {
            byte[] length = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i++)
                length[i] = message[i];
            byte[] encryptedKey = new byte[BitConverter.ToInt32(length, 0)];

            for (int i = 0; i < encryptedKey.Length; i++)
                encryptedKey[i] = message[i + sizeof(int)];
            BigInteger decryptedKey = decryptKey(new BigInteger(encryptedKey), publicKey.Key, privateKey.Key);

            byte[] data = new byte[message.Length - encryptedKey.Length - sizeof(int)];

            for (int i = 0; i < data.Length; i++)
                data[i] = message[encryptedKey.Length + sizeof(int) + i];

            return new JaCrypto().Encrypt(decryptedKey.ToByteArray(), data);
        }

        private byte[] generateRandomBigInteger()
        {
            List<byte> result = new List<byte>();
            Prng prng = new Prng((uint)new Random().Next());

            for (int i = 0; i < 4; i++)
                result.Add(prng.NextByte((byte)i));

            return result.ToArray();
        }

        private BigInteger decryptKey(BigInteger c, BigInteger n, BigInteger d)
        {
            return BigInteger.ModPow(c, d, n);
        }

        private BigInteger encryptKey(BigInteger m, BigInteger n, BigInteger e)
        {
            return BigInteger.ModPow(m, e, n);
        }

        public KeyPair GenerateKeys(BigInteger p, BigInteger q)
        {
            BigInteger n = p * q;
            BigInteger t = (p - 1) * (q - 1);
            BigInteger e = randomCoprime(t);
            BigInteger d = modularInverse(e, t);
            return new KeyPair(n, e, d);
        }

        private BigInteger randomPrime(int min, int max)
        {
            int n = 0;
            Random rnd = new Random();
            while (!isPrime((n = rnd.Next(min, max))))
                ;
            return (BigInteger)n;
        }

        private bool isPrime(int n)
        {
            if (n == 1)
                return false;
            if (n == 2)
                return true;
            int boundary = (int)Math.Floor(Math.Sqrt(n));

            for (int i = 2; i <= boundary; i++)
                if (n % i == 0)
                    return false;
            return true;
        }

        private BigInteger modularInverse(BigInteger a, BigInteger b)
        {
            BigInteger b0 = b;
            BigInteger t = 0;
            BigInteger q = 0;
            BigInteger x0 = 0;
            BigInteger x1 = 1;

            if (b == 1)
                return 1;

            while (a > 1)
            {
                q = a / b;
                t = b;
                b = a % b;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }
            if (x1 < 0)
                x1 += b0;

            return x1;
        }

        private BigInteger randomCoprime(BigInteger t)
        {
            Random rnd = new Random();

            while (true)
            {
                int start = rnd.Next(0, t > int.MaxValue ? int.MaxValue / 2 : (int)t / 2);
                int iters = rnd.Next(0, int.MaxValue);
                BigInteger i = new BigInteger(start);

                for (; i < iters && i < t; i++)
                {
                    if (gcd(i, t) == 1)
                        return i;
                }
            }
        }

        private BigInteger gcd(BigInteger a, BigInteger b)
        {
            BigInteger t;
            while (b != 0)
            {
                t = b;
                b = a % b;
                a = t;
            }
            return a;
        }
    }
}