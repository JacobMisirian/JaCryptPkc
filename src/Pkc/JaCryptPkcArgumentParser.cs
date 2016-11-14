using System;
using System.IO;

namespace Pkc
{
    public class JaCryptPkcArgumentParser
    {
        private string[] args;
        private int position;

        public JaCryptPkcConfig Parse(string[] args)
        {
            if (args.Length <= 0)
                displayHelp();

            this.args = args;

            JaCryptPkcConfig config = new JaCryptPkcConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position])
                {
                    case "-d":
                    case "--decrypt":
                        config.JaCryptPkcMode = JaCryptPkcMode.Decrypt;
                        break;
                    case "-e":
                    case "--encrypt":
                        config.JaCryptPkcMode = JaCryptPkcMode.Encrypt;
                        break;
                    case "-g":
                    case "--generate-keys":
                        config.JaCryptPkcMode = JaCryptPkcMode.GenerateKeys;
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-i":
                    case "--input-file":
                        config.InputFile = expectData("file");
                        break;
                    case "-o":
                    case "--output":
                        config.OutputFile = expectData("file");
                        break;
                    case "-k":
                    case "--key-length":
                        config.KeyLength = Convert.ToInt32(expectData("length"));
                        break;
                    case "-l":
                    case "--lower-exp":
                        config.MinExponent = Convert.ToInt32(expectData("min exp"));
                        break;
                    case "-pr":
                    case "--private-key":
                        config.PrivateKeyFile = expectData("private key");
                        break;
                    case "-pu":
                    case "--public-key":
                        config.PublicKeyFile = expectData("public key");
                        break;
                    case "-u":
                    case "--upper-exp":
                        config.MaxExponent = Convert.ToInt32(expectData("max exp"));
                        break;
                }
            }

            return config;
        }

        private void displayHelp()
        {
            Console.WriteLine("Usage: Pkc.exe [MODE] [OPTIONS] [FILES]");
            Console.WriteLine("-h --help                  Displays this help and exits");

            Console.WriteLine("\nMODE:");
            Console.WriteLine("-d --decrypt               Switches to decryption mode");
            Console.WriteLine("-e --encrypt               Switches to encryption mode");
            Console.WriteLine("-g --generate-keys         Switches to key generation mode");

            Console.WriteLine("\nOPTIONS:");
            Console.WriteLine("-k --key-length   [INT]    Sets the key length for encryption. Default 128 bytes");
            Console.WriteLine("-l --lower-exp    [INT]    Sets the 2^x for min key generation. Default 511");
            Console.WriteLine("-u --upper-exp    [INT]    Sets the 2^x for max key generation. Default 512");

            Console.WriteLine("\nFILES:");
            Console.WriteLine("-i --input-file   [FILE]   Specifies the input file");
            Console.WriteLine("-o --output       [FILE]   Specifies the output file");
            Console.WriteLine("-pr --private-key [FILE]   Specifies the private key file path");
            Console.WriteLine("-pu --public-key  [FILE]   Specifies the public key file path");

            die();
        }

        private string expectData(string type)
        {
            if (args[++position].StartsWith("-"))
                die("Expected data type {0}, instead got flag {1}!", type, args[position]);
            return args[position];
        }

        private void die(string msg = "", params object[] args)
        {
            Console.WriteLine(msg, args);
            Environment.Exit(0);
        }
    }
}

