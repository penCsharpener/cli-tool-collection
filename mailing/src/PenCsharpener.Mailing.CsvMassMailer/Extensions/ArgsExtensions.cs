using Cocona.Command;
using Cocona.CommandLine;
using PenCsharpener.Extensions;
using PenCsharpener.Mailing.Common.Models;

namespace PenCsharpener.Mailing.CsvMassMailer.Extensions;

public static class ArgsExtensions
{
    public static SmtpConfiguration? ParseSmtpConfiguration(this string[]? args)
    {
        var parser = new CoconaCommandLineParser();
        var parsed = parser.ParseCommand(args, new CommandOptionDescriptor[]
                {
                    CreateCommandOption(typeof(int), "port", new [] { 'p' }, "", new CoconaDefaultValue(465)),
                    CreateCommandOption(typeof(string), "host", new [] { 'h' }, "", new CoconaDefaultValue(string.Empty)),
                    CreateCommandOption(typeof(string), "username", new [] { 'u' }, "", new CoconaDefaultValue(string.Empty)),
                    CreateCommandOption(typeof(string), "sender-name", new [] { 'n' }, "", new CoconaDefaultValue(string.Empty)),
                    CreateCommandOption(typeof(string), "sender-address", new [] { 'a' }, "", new CoconaDefaultValue(string.Empty)),
                    CreateCommandOption(typeof(string), "password", new [] { 's' }, "", new CoconaDefaultValue(string.Empty)),
                },
                new CommandArgumentDescriptor[] { });

        var host = default(string);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "host") is { } optionHost)
        {
            host = optionHost.Value;
        }

        var password = default(string);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "password") is { } optionPassword)
        {
            password = optionPassword.Value;
        }

        var smtpSenderAddress = default(string);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "sender-address") is { } optionsmtpSenderAddress)
        {
            smtpSenderAddress = optionsmtpSenderAddress.Value;
        }

        var smtpSenderName = default(string);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "sender-name") is { } optionsmtpSenderName)
        {
            smtpSenderName = optionsmtpSenderName.Value;
        }

        var smtpUsername = default(string);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "username") is { } optionsmtpUsername)
        {
            smtpUsername = optionsmtpUsername.Value;
        }

        var port = default(int);
        if (parsed.Options.FirstOrDefault(o => o.Option.Name == "port") is { } optionPort)
        {
            port = int.Parse(optionPort.Value);
        }

        return port == 0 || host.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace() || smtpSenderAddress.IsNullOrWhiteSpace() || smtpUsername.IsNullOrWhiteSpace()
            ? null
            : new SmtpConfiguration()
            {
                SmtpHost = host,
                SmtpPassword = password,
                SmtpPort = port,
                SmtpSenderAddress = smtpSenderAddress,
                SmtpSenderName = smtpSenderName.IsNullOrWhiteSpace() ? smtpSenderAddress : smtpSenderName,
                SmtpUsername = smtpUsername
            };
    }

    private static CommandOptionDescriptor CreateCommandOption(Type optionType, string name, IReadOnlyList<char> shortName, string description, CoconaDefaultValue defaultValue, CommandOptionFlags flags = CommandOptionFlags.None)
    {
        return new CommandOptionDescriptor(optionType, name, shortName, description, defaultValue, null, flags, Array.Empty<Attribute>());
    }
}
