namespace PhotoRename.Common.Models;

public record FileName(string Name, string FileExtension)
{
    public FileName(string fileName) : this(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName))
    {
    }

    public string FullName => Name + "." + FileExtension;
}
