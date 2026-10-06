namespace PhotoRenamer.Cli.Services;

public readonly record struct FileTimestamps(DateTime CreationUtc, DateTime LastAccessUtc, DateTime LastWriteUtc);

public static class FileTimes
{
    public static FileTimestamps Read(string path)
    {
        var info = new FileInfo(path);

        return new FileTimestamps(info.CreationTimeUtc, info.LastAccessTimeUtc, info.LastWriteTimeUtc);
    }

    /// <summary>Copies creation, last access and last write time from one file to another.</summary>
    public static void Copy(string sourcePath, string targetPath)
    {
        Apply(targetPath, Read(sourcePath));
    }

    public static void Apply(string targetPath, FileTimestamps times)
    {
        // creation time first: on Unix setting it also changes the write time, so the write time has to come after
        File.SetCreationTimeUtc(targetPath, times.CreationUtc);
        File.SetLastAccessTimeUtc(targetPath, times.LastAccessUtc);
        File.SetLastWriteTimeUtc(targetPath, times.LastWriteUtc);
    }
}
