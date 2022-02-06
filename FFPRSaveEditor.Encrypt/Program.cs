using FFPRSaveEditor.Common;

namespace FFPRSaveEditor.Encrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: FFPRSaveEditor.Decrypt.exe <input.json> [output]");
                Environment.Exit(0);
            }

            string outputFilename = args.Length > 1 ? args[1] : args[0].Replace(".JSON", "").Replace(".json", "");

            try
            {
                string jsonData = File.ReadAllText(args[0]);
                jsonData = SaveGame.Encrypt(jsonData);
                File.WriteAllText(outputFilename, jsonData, System.Text.Encoding.UTF8);
                Console.WriteLine($"Successfully encrypted {jsonData.Length} byte(s).");
                Console.WriteLine($"Remember to backup your save game files!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
        }
    }
}
