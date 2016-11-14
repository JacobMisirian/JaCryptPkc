using System;
using System.IO;
using System.Numerics;

using Pkc.Cryptography;

namespace Pkc
{
    public class JaCryptPkcConfig
    {
        public JaCryptPkcMode JaCryptPkcMode { get; set; }

        public string PublicKeyFile { get; set; }
        public string PrivateKeyFile { get; set; }

        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        public void Execute()
        {
            verifyConfig();

            JaCryptPkc crypto = new JaCryptPkc();

            PublicKey publicKey;
            PrivateKey privateKey;

            switch (JaCryptPkcMode)
            {
                case JaCryptPkcMode.Decrypt:
                    publicKey = PublicKey.FromFile(PublicKeyFile);
                    privateKey = PrivateKey.FromFile(PrivateKeyFile);
                    File.WriteAllBytes(OutputFile, crypto.Decrypt(File.ReadAllBytes(InputFile), publicKey, privateKey));
                    break;
                case JaCryptPkcMode.Encrypt:
                    publicKey = PublicKey.FromFile(PublicKeyFile);
                    File.WriteAllBytes(OutputFile, crypto.Encrypt(File.ReadAllBytes(InputFile), publicKey));
                    break;
                case JaCryptPkcMode.GenerateKeys:
                    var keyPair = crypto.GenerateKeys();
                    keyPair.PublicKey.Save(PublicKeyFile);
                    keyPair.PrivateKey.Save(PrivateKeyFile);
                    break;
            }
        }

        private void verifyConfig()
        {
            if (PublicKeyFile == string.Empty || PublicKeyFile == null)
                PublicKeyFile = promptForString("Enter path for public key file: ");
            if (JaCryptPkcMode != JaCryptPkcMode.Encrypt && (PrivateKeyFile == string.Empty || PrivateKeyFile == null))
                PrivateKeyFile = promptForString("Enter path for private key file: ");

            if (JaCryptPkcMode == JaCryptPkcMode.Decrypt && !File.Exists(PrivateKeyFile))
                die("Private key file {0} does not exist!", PrivateKeyFile);

            if (JaCryptPkcMode != JaCryptPkcMode.GenerateKeys)
            {
                if (InputFile == string.Empty || InputFile == null)
                    InputFile = promptForString("Enter path for input file: ");
                if (OutputFile == string.Empty || OutputFile == null)
                    OutputFile = promptForString("Enter path for output file: ");
                if (!File.Exists(PublicKeyFile))
                    die("Public key file {0} does not exist!", PublicKeyFile);
                if (!File.Exists(InputFile))
                    die("Input file {0} does not exist!", InputFile);
            }
        }

        private BigInteger promptForBigInt(string msg = "")
        {
            Console.Write(msg);
            return BigInteger.Parse(Console.ReadLine());
        }
        private string promptForString(string msg = "")
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        private void die(string msg = "", params object[] args)
        {
            Console.WriteLine(msg, args);
            Environment.Exit(0);
        }
    }

    public enum JaCryptPkcMode
    {
        Decrypt,
        Encrypt,
        GenerateKeys
    }
}

