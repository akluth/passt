using System;
using System.IO;
using System.Windows.Forms;
using Tommy;

namespace pass
{
    static class PasswordStore
    {
        private static readonly string passFileName = ".pass";

        private static readonly string passFile =
            Environment.GetEnvironmentVariable("homedrive")
            + Environment.GetEnvironmentVariable("homepath")
            + "\\"
            + passFileName;

        public static void Init()
        {
            if (!File.Exists(passFile))
            {
                File.WriteAllText(passFile, "[pass]");

            }

            else
            {
                File.Decrypt(passFile);
            }
        }

        public static void DeInit()
        {
            File.Encrypt(passFile);
        }

        public static int Add(string title)
        {
            Console.WriteLine(title);
            Console.Write("Username: ");
            string username = Console.ReadLine();

            string password = "";
            Console.Write("Password: ");
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }

            while (keyInfo.Key != ConsoleKey.Enter);

            TomlTable newTableEntry = new TomlTable
            {
                [title] =
                {
                    ["username"] = username,
                    ["password"] = password
                }
            };

            using (StreamWriter sw = File.AppendText(passFile))
            {
                newTableEntry.ToTomlString(sw);
            }

            Console.WriteLine("\nAdded " + title + " to password store.");

            return 0;
        }

        public static int Remove(string title)
        {
            TomlTable updatedTable = DeletePasswordEntryFromTable(title);
            if (updatedTable == null) { return 1; }

            using (StreamWriter sw = new StreamWriter(passFile))
            {
                updatedTable.ToTomlString(sw);
            }

            Console.WriteLine("Removed " + title + " from password store.");

            return 0;
        }

        public static int Get(string title)
        {
            GetPasswordEntryFromTable(title);
            return 0;
        }

        private static bool GetPasswordEntryFromTable(string title)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(passFile)))
            {
                TomlTable table = TOML.Parse(reader);

                if (table.HasKey(title))
                {
                    table.TryGetNode(title, out TomlNode node);
                    Clipboard.SetText(node["password"]);
                    return true;
                }
            }

            return false;
        }

        private static TomlTable DeletePasswordEntryFromTable(string title)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(passFile)))
            {
                TomlTable table = TOML.Parse(reader);

                if (table.HasKey(title))
                {
                    table.Delete(title);
                    return table;
                }
            }

            return null;
        }
    }
}
