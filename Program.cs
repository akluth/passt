using CommandLine;
using System;

namespace pass
{
    interface IOptions
    {
        [Value(0, MetaName = "Entry title",
            HelpText = "Title of the new password entry",
            Required = true)]
        string Title { get; set; }
    }

    [Verb("add", HelpText = "Add new password to the store")]
    class AddOptions : IOptions
    {
        public string Title { get; set; }
    }

    [Verb("remove", HelpText = "Remove a password from the store")]
    class RemoveOptions : IOptions
    {
        public string Title { get; set; }
    }

    [Verb("get", HelpText = "Get a password from the store")]
    class GetOptions : IOptions
    {
        public string Title { get; set; }
    }

    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            PasswordStore.Init();

            int result = Parser.Default.ParseArguments<AddOptions, RemoveOptions, GetOptions>(args)
                .MapResult(
                  (AddOptions opts) => PasswordStore.Add(opts.Title),
                  (RemoveOptions opts) => PasswordStore.Remove(opts.Title),
                  (GetOptions opts) => PasswordStore.Get(opts.Title),
                  errs => 1);

            PasswordStore.DeInit();

            return result;
        }
    }
}
