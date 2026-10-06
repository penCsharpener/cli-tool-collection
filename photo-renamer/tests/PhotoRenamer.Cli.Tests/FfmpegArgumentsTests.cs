using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Tests;

public class FfmpegArgumentsTests
{
    private static readonly VideoEncodeOptions Options = new(28, "p5", 192);

    [Fact]
    public void BuildEncode_Uses_Nvenc_Cq_Aac_And_Keeps_Source_Framerate()
    {
        var probe = new VideoProbe("yuv420p", "tv", "bt709", "bt709", "bt709", 10);

        var args = FfmpegArguments.BuildEncode("in.mp4", "out.mp4", probe, Options);
        var line = string.Join(' ', args);

        line.Should().Contain("-c:v hevc_nvenc").And.Contain("-rc vbr -cq 28 -b:v 0").And.Contain("-preset p5");
        line.Should().Contain("-c:a aac -b:a 192k").And.Contain("-map_metadata 0").And.Contain("-fps_mode passthrough").And.Contain("-metadata photorenamer=h265");
        line.Should().Contain("-color_range tv").And.Contain("-colorspace bt709").And.Contain("-color_primaries bt709").And.Contain("-color_trc bt709");
        line.Should().NotContain(" -r ").And.NotContain("p010le");
        args[args.IndexOf("-i") + 1].Should().Be("in.mp4");
        args.Last().Should().Be("out.mp4");
    }

    [Fact]
    public void BuildEncode_Switches_To_10_Bit_And_Skips_Unknown_Colour_Info()
    {
        var probe = new VideoProbe("yuv420p10le", "pc", "unknown", null, "unspecified", 10);

        var line = string.Join(' ', FfmpegArguments.BuildEncode("in.mov", "out.mp4", probe, Options));

        line.Should().Contain("-pix_fmt p010le -profile:v main10").And.Contain("-color_range pc");
        line.Should().NotContain("-colorspace").And.NotContain("-color_trc").And.NotContain("-color_primaries");
    }
}
