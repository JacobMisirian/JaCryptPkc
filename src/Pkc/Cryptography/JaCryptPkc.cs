using System;
using System.Collections.Generic;
using System.Numerics;

using JaCrypt.Cryptography;

namespace Pkc.Cryptography
{
    public class JaCryptPkc
    {
        private Random rnd;

        public JaCryptPkc()
        {
            rnd = new Random();
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

        public byte[] Encrypt(byte[] message, PublicKey publicKey, int keyLength = 128)
        {
            List<byte> result = new List<byte>();
            byte[] key = generateRandomBigInteger(keyLength);
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

        public KeyPair GenerateKeys(int minExp = 511, int maxExp = 512)
        {
            return GenerateKeys(generateRandomPrime(2, minExp, 2, maxExp), generateRandomPrime(2, minExp, 2, maxExp));
        }
        public KeyPair GenerateKeys(BigInteger p, BigInteger q)
        {
            BigInteger n = p * q;
            BigInteger t = (p - 1) * (q - 1);
            BigInteger e = randomCoprime(t);
            BigInteger d = modularInverse(e, t);
            return new KeyPair(n, e, d);
        }

        private BigInteger decryptKey(BigInteger c, BigInteger n, BigInteger d)
        {
            return BigInteger.ModPow(c, d, n);
        }
        private BigInteger encryptKey(BigInteger m, BigInteger n, BigInteger e)
        {
            return BigInteger.ModPow(m, e, n);
        }

        private byte[] generateRandomBigInteger(int size)
        {
            List<byte> result = new List<byte>();
            byte[] b = new byte[1];
            for (int i = 0; i < size; i++)
            {
                rnd.NextBytes(b);
                result.Add(b[0]);
            }

            return result.ToArray();
        }

        private BigInteger generateRandomPrime(int minBase, int minExp, int maxBase, int maxExp)
        {
            BigInteger min = BigInteger.Pow(minBase, minExp);
            BigInteger max = BigInteger.Pow(maxBase, maxExp);

            while (++min < max)
            {
                if (isPrime(min, 10))
                {
                    if (rnd.Next(0, 10) == 1)
                        return min;
                }
            }
            return -1;
        }

        private bool isPrime(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;

            BigInteger d = n - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }
            BigInteger a;

            for (int i = 0; i < k; i++)
            {
                do
                {
                    a = new BigInteger(generateRandomBigInteger(n.ToByteArray().Length));
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;

                for (int j = 0; j < s; j++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        return false;
                }

                if (x != n - 1)
                    return false;
            }
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