using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TransComp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                int arg_verbose = SearchArg(args, "-v");
                int arg_comp = SearchArg(args, "-c");
                int arg_decomp = SearchArg(args, "-d");
                int arg_input = SearchArg(args, "-i");
                int arg_output = SearchArg(args, "-o");
                int arg_addr = SearchArg(args, "-a");

                if (arg_comp != -1 && arg_decomp != -1)
                {
                    ShowVersion();
                    Console.WriteLine("Error: Both Compress and Decompress?");
                }
                else if (arg_comp == -1 && arg_decomp == -1)
                {
                    ShowVersion();
                    Console.WriteLine("Error: No Compress and Decompress");
                }

                if (arg_input == -1)
                {
                    ShowVersion();
                    Console.WriteLine("Error: No input file");
                }

                if (arg_output == -1)
                {
                    ShowVersion();
                    Console.WriteLine("Error: No input file");
                }

                string compformat = args[Math.Max(arg_comp, arg_decomp) + 1];
                string inputfile = args[arg_input + 1];
                string outputfile = args[arg_output + 1];
                bool isComp = (arg_comp != -1);

                if (isComp)
                {
                    //Compression
                }
                else
                {
                    //Decompression
                    byte[] outbuf = new byte[0];

                    if (compformat == "lz2")
                    {
                        Decomp.ISPK.LC_LZ2(File.ReadAllBytes(inputfile), 0, out outbuf);
                    }

                    if (outbuf.Length > 0)
                        File.WriteAllBytes(outputfile, outbuf);
                }

            }
            else
            {
                ShowVersion();
                Usage();
            }
        }

        static int SearchArg(string[] args, string search)
        {
            for (int i = 0; i < args.Length; i++)
                if (args[i] == search)
                    return i;
            return -1;
        }

        static void Usage()
        {
            Console.WriteLine("TransComp.exe -c/d [format] -i [input file] -o [output file] (-a <address in input/output file>) (-v)");
            Console.WriteLine("    -c [format] - Compress Input file to Output file.");
            Console.WriteLine("    -d [format] - Decompress Input file to Output file.");
            Console.WriteLine("    -i [input file] - Set Input file.");
            Console.WriteLine("    -o [output file] - Set Output file.");
            Console.WriteLine("    -a [address] - Set address from Input / to Output file. (Optional, default = 0)");
            Console.WriteLine("    -v - Verbose");
        }

        static void ShowVersion()
        {
            Console.WriteLine("TransComp v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
