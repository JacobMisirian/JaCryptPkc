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
                    case "-pr":
                    case "--private-key":
                        config.PrivateKeyFile = expectData("private key");
                        break;
                    case "-pu":
                    case "--public-key":
                        config.PublicKeyFile = expectData("public key");
                        break;
                }
            }

            return config;
        }

        private void displayHelp()
        {
            Console.WriteLine("Usage: Pkc.exe [MODE] [FILES]");
            Console.WriteLine("-h --help                  Displays this help and exits.");

            Console.WriteLine("\nMODE:");
            Console.WriteLine("-d --decrypt               Switches to decryption mode.");
            Console.WriteLine("-e --encrypt               Switches to encryption mode.");
            Console.WriteLine("-g --generate-keys         Switches to key generation mode.");

            Console.WriteLine("\nFILES:");
            Console.WriteLine("-i --input-file [FILE]     Specifies the input file.");
            Console.WriteLine("-o --output [FILE]         Specifies the output file.");
            Console.WriteLine("-pr --private-key [FILE]   Specifies the private key file path.");
            Console.WriteLine("-pu --public-key [FILE]    Specifies the public key file path.");

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

