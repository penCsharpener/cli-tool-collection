namespace PhotoRename.Common.Models;

public record RenamePair
{
    public RenamePair(string originalFilePath, string newFileName)
    {
        OriginalFilePath = originalFilePath;
        FileInfo = new FileInfo(originalFilePath);
        NewFileInfo = new FileInfo(Path.Combine(FileInfo.Directory!.FullName, newFileName));
        NewFileName = newFileName;

        var oldNameFormat = GlobalOptions.OnlyUseFilename ? FileInfo.Name : FileInfo.FullName;
        var newNameFormat = GlobalOptions.OnlyUseFilename ? NewFileInfo.Name : NewFileInfo.FullName;

        CmdRenameCommand = $"ren \"{oldNameFormat}\" \"{newNameFormat}\"";
        PowershellRenameCommand = $"Rename-Item -Path \"{oldNameFormat}\" -NewName \"{newNameFormat}\"";
    }

    public string OriginalFilePath { get; set; }
    public string NewFileName { get; set; }
    public FileInfo FileInfo { get; }
    public FileInfo NewFileInfo { get; set; }
    public string CmdRenameCommand { get; }
    public string PowershellRenameCommand { get; }
}
