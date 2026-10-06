namespace PhotoRenamer.Cli.Services;

/// <summary>Marks original files that were converted by appending "__org" to the file name, and restores the name later.</summary>
public static class OriginalMarker
{
    public const string Suffix = "__org";

    public static bool IsMarked(string path)
    {
        return Path.GetFileNameWithoutExtension(path).EndsWith(Suffix, StringComparison.OrdinalIgnoreCase);
    }

    public static string GetMarkedPath(string path)
    {
        return Path.Combine(Path.GetDirectoryName(path)!, Path.GetFileNameWithoutExtension(path) + Suffix + Path.GetExtension(path));
    }

    public static string GetRestoredPath(string path)
    {
        var stem = Path.GetFileNameWithoutExtension(path);

        return Path.Combine(Path.GetDirectoryName(path)!, stem[..^Suffix.Length] + Path.GetExtension(path));
    }

    /// <summary>Renames the file to its marked name. Returns the new path, or null if nothing was done (already marked, or the marked name is taken).</summary>
    public static string? Mark(string path)
    {
        return IsMarked(path) ? null : Move(path, GetMarkedPath(path));
    }

    /// <summary>Removes the marker from the file name. Returns the new path, or null if the restored name is already taken.</summary>
    public static string? Restore(string path)
    {
        return IsMarked(path) ? Move(path, GetRestoredPath(path)) : null;
    }

    private static string? Move(string source, string target)
    {
        if (File.Exists(target))
        {
            return null;
        }

        File.Move(source, target); // a rename keeps EXIF and file times untouched

        return target;
    }
}
