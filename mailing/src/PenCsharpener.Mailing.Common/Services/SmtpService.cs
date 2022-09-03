using Microsoft.Extensions.Options;
using PenCsharpener.Mailing.Common.Models;
using PenCsharpener.Mailing.Common.Services.Abstractions;

namespace PenCsharpener.Mailing.Common.Services;

public class SmtpService : ISmtpService
{
	private readonly SmtpConfiguration _options;

	public SmtpService(IOptions<SmtpConfiguration> options)
	{
		_options = options.Value;
	}
}
