using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

using System;

/// <summary>
/// Execute an external .exe program
/// </summary>
class ExeRunner
{
    #region Thread Attributes
    private ManualResetEvent _doneEvent;
    #endregion
    
    #region classAttributes
    // Current Working Directory
            // Getting the current directory path
        // source: https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c
    private static string startupPath = System.IO.Directory.GetCurrentDirectory() + "\\ExePrograms\\";

    // External Exe Location
    private string mainExe;

    //Parameters
    List<string[]> parameters = new List<string[]>();

    // Exe Running/Working Directory
    private string workingDirectory;

    // Exe Output
    private string output;

    // Exe Exception
    private string exception;        
    #endregion

    #region Gettes&Setters
        
    /// <summary>
    /// Location of the external exe file
    /// </summary>
    /// <returns>string</returns>
    public string MainExe { get => mainExe; set => mainExe = value; }


    /// <summary>
    /// Parameter List. 
    /// Example:
    /// ((--ffmpeg-location, C:\\Users\\Jeff...),(--extract-audio, ),(--audio-format, mp3)).
    /// The length of the inner array will be specified when the list object is created.
    /// </summary>
    public List<string[]> Parameters { get => parameters; set => parameters = value; }

    /// <summary>
    /// New working directory where the exe will run
    /// </summary>
    /// <returns></returns>
    public string WorkingDirectory { get => workingDirectory; set => workingDirectory = value; }


    /// <summary>
    /// Stores the output of the exe
    /// </summary>
    /// <returns>string</returns>
    public string Output { get => output; set => output = value; }


    /// <summary>
    /// Stores the Exceptions/Errors of the exe
    /// </summary>
    /// <returns>string</returns>
    public string Exception { get => exception; set => exception = value; }

    /// <summary>
    /// Changes the directory where the program searches for the exe
    /// </summary>
    /// <returns></returns>
    public static string StartupPath { get => startupPath; set => startupPath = value; }
    

    /// <summary>
    /// Defines the Event handler for the threadingpool to use
    /// </summary>
    /// <returns></returns>
    public ManualResetEvent DoneEvent { get => _doneEvent; set => _doneEvent = value; }


    #endregion

    #region Constructors
    /// <summary>
    /// Uses the current directory and searches for the "ExePrograms" directory, But runs the program elsewhere. NOTE: THIS DOES NOT USE THREADING, USE A DIFFERENT CONSTRUCTOR FOR THIS PURPOSE.
    /// </summary>
    public ExeRunner(string mainExeName, string newRunningDirectory){
            MainExe = mainExeName;
            WorkingDirectory = newRunningDirectory;
        }

    /// <summary>
    /// Uses the current directory and searches for the "ExePrograms" directory, But runs the program elsewhere. NOTE: THIS CONSTRUCTOR IS FOR EXECUTING THE METHOD ON A THREADPOOL.
    /// </summary>
    public ExeRunner(string mainExeName, string newRunningDirectory, ManualResetEvent doneEvent){
            MainExe = mainExeName;
            WorkingDirectory = newRunningDirectory;
            DoneEvent = doneEvent;
        }


    /// <summary>
    /// Uses the current directory and searches for the "ExePrograms" directory.
    /// </summary>
    public ExeRunner(string mainExeName){
            MainExe = mainExeName;
            WorkingDirectory = StartupPath;
        }

    /// <summary>
    /// Uses a custom directory.
    /// </summary>
    public ExeRunner(string mainExeName, string newRunningDirectory, string customDirectory){
            MainExe = mainExeName;
            WorkingDirectory = newRunningDirectory;
            StartupPath = customDirectory;
        }
    #endregion




    #region Methods
    /// <summary>
    /// Method that assembles the full parameter that is to be sent to the external exe. The "parameters" attribute needs to be previously initialized.
    /// </summary>
    /// <returns></returns>
    private string BuildFullParameter(){
        string fullParameter = "";
        foreach (string[] parameter in Parameters)
        {
            for (int i = 0; i < parameter.Length; i++)
            {
                fullParameter += parameter[i] + " ";
            }
        }

        return fullParameter;
    }

    /// <summary>
    /// Method that executes the external program
    /// </summary>
    /// Source: <see cref="https://stackoverflow.com/questions/9679375/run-an-exe-from-c-sharp-code"/>
    public void Execute(){
        
    // Use ProcessStartInfo class
        // example: https://msdn.microsoft.com/es-es/library/system.diagnostics.processstartinfo.redirectstandardoutput(v=vs.110).aspx#Ejemplos
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Maximized;
        startInfo.FileName = StartupPath + this.MainExe;
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardOutput = true;

        FolderCreator.CreateFolder(this.WorkingDirectory + "\\Results" );

        startInfo.WorkingDirectory = this.WorkingDirectory + "\\Results" ;
        startInfo.Arguments = this.BuildFullParameter();
        Console.WriteLine(startInfo.Arguments);
        try
        {
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            //Reading output and errors:
                //https://msdn.microsoft.com/es-es/library/system.diagnostics.processstartinfo.redirectstandardoutput(v=vs.110).aspx#Ejemplos
            using (Process exeProcess = Process.Start(startInfo))
            {
                Console.WriteLine("Salida:\n----------------------\n");
                Console.WriteLine(exeProcess.StandardOutput.ReadToEnd());

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
    #endregion

    #region ThreadCallBackMethods
    public void ThreadPoolExecuteCallback(Object threadContext){
    
        if (_doneEvent != null)
        {
            Console.WriteLine("You must provide a ManualResetEvent --> this.DoneEvent");
        }
        else{
            // store the thread index
            int threadIndex = (int)threadContext;
            Execute();
            Console.WriteLine("thread {0} result calculated...", threadIndex);
            
            // Notifies the threadpool that this thread has finished its execution
            _doneEvent.Set();
        }

    }
    #endregion
}