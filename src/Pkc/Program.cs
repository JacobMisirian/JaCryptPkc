using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

using Pkc.Cryptography;

namespace Pkc
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new JaCryptPkcArgumentParser().Parse(args).Execute();
        }
    }
}