namespace PhotoRename.Common.Models;

public record RenamePair
{
    public RenamePair(string originalFilePath, string newFileName)
    {
        OriginalFilePath = originalFilePath;
        FileInfo = new FileInfo(originalFilePath);
        NewFileInfo = new FileInfo(Path.Combine(FileInfo.Directory!.FullName, newFileName));
        NewFileName = newFileName;

        var oldNameFormat = FileInfo.Name;
        var newNameFormat = NewFileInfo.Name;

        CmdRenameCommand = $"ren \"{oldNameFormat}\" \"{newNameFormat}\"";
        PowershellRenameCommand = $"Rename-Item -Path \"{oldNameFormat}\" -NewName \"{newNameFormat}\"";
    }

    public string OriginalFilePath { get; set; }
    public string NewFileName { get; set; }
    public FileInfo FileInfo { get; }
    public FileInfo NewFileInfo { get; set; }
    public string CmdRenameCommand { get; private set; }
    public string PowershellRenameCommand { get; private set; }

    public RenamePair ApplyOptions(bool onlyUseFileName)
    {
        var oldNameFormat = onlyUseFileName ? FileInfo.Name : FileInfo.FullName;
        var newNameFormat = onlyUseFileName ? NewFileInfo.Name : NewFileInfo.FullName;

        CmdRenameCommand = $"ren \"{oldNameFormat}\" \"{newNameFormat}\"";
        PowershellRenameCommand = $"Rename-Item -Path \"{oldNameFormat}\" -NewName \"{newNameFormat}\"";

        return this;
    }
}
