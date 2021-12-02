using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sexyhiker
{
    class Decompiler
    {
        private bool verbose;
        private byte[] key;

        public Decompiler(bool verbose)
        {
            this.verbose = verbose;
        }

        public void Decompile(string readPath, string writePath)
        {
            using (FileStream stream = new FileStream(readPath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                //Skip unimportant bytes
                if (verbose) Console.WriteLine("Skipping header");
                reader.BaseStream.Seek(1250000L, SeekOrigin.Begin);

                //Check header thing
                if (reader.ReadInt32() != 1230500)
                {
                    Console.WriteLine("Incorrect file type");
                    return;
                }
                if (verbose) Console.WriteLine("Correct file type");

                //Generates decryption key
                if (verbose) Console.WriteLine("Generating Key/Lookup");
                key = generateLookup(reader.ReadInt32());
                reader.BaseStream.Seek(4L, SeekOrigin.Current);
                reader.BaseStream.Seek(reader.ReadInt32(), SeekOrigin.Current);

                //Writes the GMD file
                if (verbose) Console.WriteLine("Writing to file");
                using (FileStream writeStream = new FileStream(writePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(writeStream))
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                        writer.Write(decrypt(reader.ReadByte()));
            }
        }

        public byte decrypt(byte byteToDecrypt) => key[byteToDecrypt];

        public byte[] generateLookup(int param)
        {
            byte[][] lookup =
            {
                new byte[256],
                new byte[256]
            };

            for (int i = 0; i < 256; i++)
            {
                lookup[0][i] = (byte)i;
            }

            for (int i = 1; i < 10001; i++)
            {
                int index = i * param % 254 + 1;
                byte val = lookup[0][index];

                lookup[0][index] = lookup[0][index + 1];
                lookup[0][index + 1] = val;
            }

            for (int i = 1; i < 256; i++)
                lookup[1][lookup[0][i]] = (byte)i;

            return lookup[1];
        }
    }
}
