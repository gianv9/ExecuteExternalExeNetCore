using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        LaunchCommandLineApp();
    }


    /// <summary>
    /// Launch the legacy application with some options set.
    /// Fuente del programa: <see cref="https://stackoverflow.com/questions/9679375/run-an-exe-from-c-sharp-code"/>
    /// </summary>
    static void LaunchCommandLineApp()
    {
        // Getting the current directory path
        // source: https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c

        string startupPath = System.IO.Directory.GetCurrentDirectory() + "\\ExePrograms\\";

        //string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +"\\ExePrograms\\";

        // For the example
        const string youtube = "youtube-dl.exe";
        const string ffmpeg = "ffmpeg.exe";
        string mainParam = "https://www.youtube.com/playlist?list=PLXxkUINeOOXsdBHXsJP1Spf0HpblUF_Bs";

        // Use ProcessStartInfo class
        // example: https://msdn.microsoft.com/es-es/library/system.diagnostics.processstartinfo.redirectstandardoutput(v=vs.110).aspx#Ejemplos
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = false;
        startInfo.FileName = startupPath + youtube;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.RedirectStandardError = true;
        //startInfo.RedirectStandardOutput = true;
        startInfo.WorkingDirectory =startupPath + "\\Results" ;
        startInfo.Arguments = "--ffmpeg-location \"" + @startupPath + ffmpeg + "\" --extract-audio --audio-format mp3 --yes-playlist " + mainParam;

        try
        {
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            //Reading output and errors:
                //https://msdn.microsoft.com/es-es/library/system.diagnostics.processstartinfo.redirectstandardoutput(v=vs.110).aspx#Ejemplos
            using (Process exeProcess = Process.Start(startInfo))
            {
                //Console.WriteLine("Salida:\n----------------------\n");
                //Console.WriteLine(exeProcess.StandardOutput.ReadToEnd());

                exeProcess.WaitForExit();

                Console.WriteLine("Errores:\n----------------------\n");
                Console.WriteLine(exeProcess.StandardError.ReadToEnd());
            }
            //Console.ReadKey();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            //Console.ReadKey();
            // Log error.
        }
    }
}