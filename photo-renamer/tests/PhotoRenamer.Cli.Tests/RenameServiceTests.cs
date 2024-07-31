using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoRename.Common.Services.Abstractions;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Tests;

public class RenameServiceTests
{
    private readonly RenameService _sut;
    private readonly IFileService _fileService;
    private readonly IImageSharpWrapper _imageSharpWrapper;
    private readonly IHostEnvironment _hostingEnvironment;
    private readonly IServiceProvider _serviceProvider;

    private static readonly string[] _fileNames = new[] {
                    "20240730_131415.JPG",
                    "20240730_131415.MP4",
                    "IMG_1234.JPG",
                    "IMG_2345 with tags.JPG",
                    "VID_20240730_111213999.MP4",
                    "IMG_20240706_171900970_HDR.JPG",
                    "IMG_20240708_150142259.jpg",
                    "IMG_20240706_171900970_HDR with tag.JPG",
                    "IMG_20240708_150142259 with tag.jpg"
                };

    public RenameServiceTests()
    {
        _imageSharpWrapper = Substitute.For<IImageSharpWrapper>();
        var services = new ServiceCollection()
            .AddScoped(_ =>
            {
                var service = Substitute.For<IFileService>();
                service.GetFiles(Arg.Any<string>(), Arg.Any<string[]>()).Returns(_fileNames);

                return service;
            })
            .AddScoped(_ =>
            {
                var service = Substitute.For<IImageSharpWrapper>();
                service.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

                return service;
            })
            .AddScoped(_ =>
            {
                var service = Substitute.For<IHostEnvironment>();
                service.ContentRootPath.Returns("E:\\");

                return service;
            })
            .AddScoped<IFileNameStrategyFactory, FileNameStrategyFactory>()

            .AddScoped<RenameService>()
        ;

        _serviceProvider = services.BuildServiceProvider();

        _sut = _serviceProvider.GetRequiredService<RenameService>();
    }

    [Fact]
    public async Task Test()
    {
        var cmds = _sut.GetNameCommandsAsync(new RenameParameters() { OnlyUseFilename = true }, CancellationToken.None);
        var list = new List<string>();

        await foreach (var cmd in cmds)
        {
            Debug.WriteLine(cmd);
            list.Add(cmd);
        }

        list.Should().BeEquivalentTo(new[]
        {
            @"Rename-Item -Path ""IMG_1234.JPG"" -NewName ""20240730_111213_IMG_1234.JPG""",
            @"Rename-Item -Path ""IMG_2345 with tags.JPG"" -NewName ""20240730_111213_IMG_2345 with tags.JPG""",
            @"Rename-Item -Path ""VID_20240730_111213999.MP4"" -NewName ""20240730_111213_VID.MP4""",
            @"Rename-Item -Path ""IMG_20240706_171900970_HDR.JPG"" -NewName ""20240706_171900970_IMG_HDR.JPG""",
            @"Rename-Item -Path ""IMG_20240708_150142259.jpg"" -NewName ""20240708_150142259_IMG.jpg""",
            @"Rename-Item -Path ""IMG_20240706_171900970_HDR with tag.JPG"" -NewName ""20240706_171900970_IMG_HDR with tag.JPG""",
            @"Rename-Item -Path ""IMG_20240708_150142259 with tag.jpg"" -NewName ""20240708_150142259_IMG with tag.jpg"""
        });
    }
}