using CommandLine;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SharpCryptr
{
    internal class Program
    {
        class FileOptions
        {
            [Option('p', "password", Required = true, HelpText = "The encryption password, make it strong")]
            public string? Password { get; set; }

            [Option('i', "input", Required = true, HelpText = "Specify the file you want to encrypt/decrypt")]
            public string? Input { get; set; }

            [Option('o', "output", Required = true, HelpText = "Specify the name of the output file")]
            public string? Output { get; set; }
        }


        [Verb("encrypt", HelpText = "Encrypt your files")]
        class EncryptOptions : FileOptions { }

        [Verb("decrypt", HelpText = "Decrypt your files")]
        class DecryptOptions : FileOptions { }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<EncryptOptions, DecryptOptions>(args)
                .WithParsed<EncryptOptions>(Encrypt)
                .WithParsed<DecryptOptions>(Decrypt);
        }

        static void Encrypt(EncryptOptions options)
        {
            Aes aes = CreateAes(options.Password);
            try
            {
                byte[] inputFile = File.ReadAllBytes(options.Input);
                byte[] cipherText = aes.EncryptCbc(inputFile, aes.IV);
                byte[] outputFile = new byte[aes.IV.Length + cipherText.Length];
                Buffer.BlockCopy(aes.IV, 0, outputFile, 0, aes.IV.Length);
                Buffer.BlockCopy(cipherText, 0, outputFile, aes.IV.Length, cipherText.Length);
                File.WriteAllBytes(options.Output, outputFile);
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine("I couldn't find your input file, are you sure this is correct: " + options.Input);
            }
            finally
            {
                aes.Dispose();
            }
        }

        static void Decrypt(DecryptOptions options)
        {
            Aes aes = CreateAes(options.Password);
            try
            {
                byte[] inputFile = File.ReadAllBytes(options.Input);
                byte[] iv = new byte[aes.IV.Length];
                byte[] cipherText = new byte[inputFile.Length - iv.Length];
                Buffer.BlockCopy(inputFile, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(inputFile, iv.Length, cipherText, 0, cipherText.Length);

                byte[] outputFile = aes.DecryptCbc(cipherText, iv);
                File.WriteAllBytes(options.Output, outputFile);
                Console.WriteLine("Done!");
            }           
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine("I couldn't find your input file, are you sure this is correct: " + options.Input);
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine("Decryption failed, probably bad password");
            }
            finally
            {
                aes.Dispose();
            }
        }

        static Aes CreateAes(string password)
        {
            using SHA256 hash = SHA256.Create();
            Aes aes = Aes.Create();

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            aes.Key = hash.ComputeHash(Encoding.ASCII.GetBytes(password));

            return aes;
        }
    }
}