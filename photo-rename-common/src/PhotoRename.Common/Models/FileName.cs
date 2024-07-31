namespace PhotoRename.Common.Models;

public record FileName
{
    public FileName(string fileName)
    {
        Name = Path.GetFileNameWithoutExtension(fileName);
        FileExtension = Path.GetExtension(fileName);
        FullPath = fileName;
    }

    public string FullName => Name + FileExtension;
    public string Name { get; }
    public string FileExtension { get; }
    public string FullPath { get; }
}
