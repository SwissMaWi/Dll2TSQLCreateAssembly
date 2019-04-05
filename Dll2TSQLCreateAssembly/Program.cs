using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Dll2TSQLCreateAssembly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2 || string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
            {
                Console.WriteLine("Usage: Dll2TSQLCreateAssembly.exe assemblyPath sqlFilePath");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"File '{args[0]}' not found.");
                return;
            }

            var builder = new StringBuilder($"CREATE ASSEMBLY [{Path.GetFileNameWithoutExtension(args[0])}] AUTHORIZATION [dbo] FROM 0x");
            using (FileStream stream = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int currentByte = stream.ReadByte();
                while (currentByte > -1)
                {
                    builder.Append(currentByte.ToString("X2", CultureInfo.InvariantCulture));
                    currentByte = stream.ReadByte();
                }
            }

            builder.Append(" WITH PERMISSION_SET = EXTERNAL_ACCESS\n");
            File.WriteAllText(args[1], builder.ToString());
            Console.WriteLine($"Done, written to '{args[1]}'.");
        }
    }
}