using System.IO;

/// <summary>
/// Create folders and verify if they exist.
/// </summary>
/// Reference: <see cref="https://msdn.microsoft.com/en-us/library/system.io.directory.exists(v=vs.110).aspx"/>
class FolderCreator
{
    public static void CreateFolder(string folderPath){
        if ( ! FolderExists(folderPath) )
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static bool FolderExists(string folder){
        if (Directory.Exists(folder))
        {
            return true;
        }
        return false;
    }
}