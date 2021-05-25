using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransComp
{
    public static class Decomp
    {
        public static class ISPK
        {
            public static int LC_LZ2(byte[] input, int addr, out byte[] output, out int csize)
            {
                List<byte> buf = new List<byte>();
                int cmd = -1;
                int len = -1;
                int i;
                for (i = addr; i < input.Length; i++)
                {
                    switch (cmd)
                    {
                        case 0x00:  //Copy
                            buf.Add(input[i]);
                            len--;
                            break;
                        case 0x01:  //Byte Fill
                            for (int j = 0; j < len; j++)
                                buf.Add(input[i]);
                            len = 0;
                            break;
                        case 0x02:  //Word Fill
                            for (int j = 0; j < len; j++)
                                buf.Add(input[i + (j & 1)]);
                            i++;
                            len = 0;
                            break;
                        case 0x03:  //Increment Fill
                            int inc = input[i];
                            for (int j = 0; j < len; j++)
                            {
                                buf.Add((byte)(inc & 0xFF));
                                inc++;
                            }
                            len = 0;
                            break;
                        case 0x04:  //Repeat
                            int rep = (input[i] * 0x100) + input[i + 1];
                            if (rep >= buf.Count)
                            {
                                Console.WriteLine("Error: 0x" + i.ToString("X") + " repeat cmd has wrong address");
                                output = new byte[0];
                                csize = 0;
                                return -1;
                            }
                            for (int j = 0; j < len; j++)
                            {
                                buf.Add(buf[rep + j]);
                            }
                            i++;
                            len = 0;
                            break;
                        default:
                            if (input[i] == 0xFF)
                            {
                                cmd = 0xFF;
                            }
                            else if (input[i] >= 0xE0)
                            {
                                cmd = (input[i] >> 2) & 7;
                                len = (input[i] & 3) * 0x100; i++;
                                len += input[i];
                                //Console.WriteLine("Test: 0x" + (i - 2).ToString("X") + " - CMD 0x" + cmd.ToString("X") + " / LEN 0x" + len.ToString("X"));
                            }
                            else
                            {
                                cmd = (input[i] >> 5) & 7;
                                len = input[i] & 0x1F;
                                //Console.WriteLine("Test: 0x" + (i - 1).ToString("X") + " - CMD 0x" + cmd.ToString("X") + " / LEN 0x" + len.ToString("X"));
                            }
                            len++;
                            break;
                    }

                    //Stop Command
                    if (len == 0)
                        cmd = -1;

                    //Stop Decomp
                    if (cmd == 0xFF)
                        break;
                }
                csize = i - addr;
                output = buf.ToArray();
                return 0;
            }

            public static int LC_LZ2M(byte[] input, int addr, out byte[] output, out int csize)
            {
                List<byte> buf = new List<byte>();
                int cmd = -1;
                int len = -1;
                int i;
                for (i = addr; i < input.Length; i++)
                {
                    switch (cmd)
                    {
                        case 0x00:  //Copy
                            buf.Add(input[i]);
                            len--;
                            break;
                        case 0x01:  //Byte Fill
                            for (int j = 0; j < len; j++)
                                buf.Add(input[i]);
                            len = 0;
                            break;
                        case 0x02:  //Word Fill
                            for (int j = 0; j < len; j++)
                                buf.Add(input[i + (j & 1)]);
                            i++;
                            len = 0;
                            break;
                        case 0x03:  //Increment Fill
                            int inc = input[i];
                            for (int j = 0; j < len; j++)
                            {
                                buf.Add((byte)(inc & 0xFF));
                                inc++;
                            }
                            len = 0;
                            break;
                        case 0x04:  //Repeat
                            int rep = (input[i] * 0x100) + input[i + 1];
                            if (rep >= buf.Count)
                            {
                                Console.WriteLine("Error: 0x" + i.ToString("X") + " repeat cmd has wrong address");
                                output = new byte[0];
                                csize = 0;
                                return -1;
                            }
                            for (int j = 0; j < len; j++)
                            {
                                buf.Add(buf[rep + j]);
                            }
                            i++;
                            len = 0;
                            break;
                        default:
                            if (input[i] == 0xFF)
                            {
                                cmd = 0xFF;
                            }
                            else if (input[i] >= 0xE0)
                            {
                                cmd = (input[i] >> 2) & 7;
                                len = (input[i] & 3) * 0x100; i++;
                                len += input[i];
                                //Console.WriteLine("Test: 0x" + (i - 2).ToString("X") + " - CMD 0x" + cmd.ToString("X") + " / LEN 0x" + len.ToString("X"));
                            }
                            else if (input[i] >= 0xC0)
                            {
                                cmd = (input[i] >> 2) & 7; i++;
                                len = input[i] * 0x100; i++;
                                len += input[i];
                                //Console.WriteLine("Test: 0x" + (i - 2).ToString("X") + " - CMD 0x" + cmd.ToString("X") + " / LEN 0x" + len.ToString("X"));
                            }
                            else
                            {
                                cmd = (input[i] >> 5) & 7;
                                len = input[i] & 0x1F;
                                //Console.WriteLine("Test: 0x" + (i - 1).ToString("X") + " - CMD 0x" + cmd.ToString("X") + " / LEN 0x" + len.ToString("X"));
                            }
                            len++;
                            break;
                    }

                    //Stop Command
                    if (len == 0)
                        cmd = -1;

                    //Stop Decomp
                    if (cmd == 0xFF)
                        break;
                }
                csize = i - addr;
                output = buf.ToArray();
                return 0;
            }
        }
    }
}
