namespace PhotoRename.Common.Models;

public record FileName
{
    public FileName(string fileName)
    {
        Name = Path.GetFileNameWithoutExtension(fileName);
        FileExtension = Path.GetExtension(fileName);
        FullPath = fileName;
        FullDirectory = new FileInfo(fileName).DirectoryName!;
    }

    public string FullName => Name + FileExtension;
    public string Name { get; }
    public string FileExtension { get; }
    public string FullPath { get; }
    public string FullDirectory { get; }
}
