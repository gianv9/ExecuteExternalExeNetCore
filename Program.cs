using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        const int numberOfSimultaneousDownloads = 3;

        #region ProgramParameters
        // This is a jagged-array
        // https://stackoverflow.com/questions/3814145/how-can-i-declare-a-two-dimensional-string-array
        string[][] videos = new string[numberOfSimultaneousDownloads][]
        { new string[]{"https://www.youtube.com/watch?v=YvCTU46FBQA"},
        new string[]{"https://www.youtube.com/watch?v=_ziPHk9iErc"},
        new string[]{"https://www.youtube.com/watch?v=7KNLw-tIv2Y"}};


        List<string[]> parametros = new List<string[]>();
        parametros.Add(new string[]{"--ffmpeg-location", "\"" + ExeRunner.StartupPath +"ffmpeg.exe" + "\"" });
        parametros.Add(new string[]{"--extract-audio"});
        parametros.Add(new string[]{"--audio-format","mp3"});
        parametros.Add(new string[]{"--no-playlist"}); 
        //parametros.Add(new string[]{"https://www.youtube.com/watch?v=bpOSxM0rNPM"});

        #endregion

        ManualResetEvent[] doneEvents = new ManualResetEvent[numberOfSimultaneousDownloads]; 
        ExeRunner[] runners = new ExeRunner[numberOfSimultaneousDownloads];

        for (int i = 0; i < numberOfSimultaneousDownloads; i++)
        {
            doneEvents[i] = new ManualResetEvent(false);
            ExeRunner youtubeDlSubProcess = new ExeRunner("youtube-dl.exe", "C:\\Users\\gian\\Music",doneEvents[i]);
            if(i != 0){
                // Eliminamos la url anterior
                parametros.RemoveAt( i-1 );
            }
            parametros.Add(videos[i]);
            youtubeDlSubProcess.Parameters = parametros;
            runners[i] = youtubeDlSubProcess;  
            ThreadPool.QueueUserWorkItem(youtubeDlSubProcess.ThreadPoolExecuteCallback, i);  
        }
       

            // Wait for all threads in pool to calculate.  
        WaitHandle.WaitAll(doneEvents);  
        Console.WriteLine("All calculations are complete.");  


        // Display the results.  
        for (int i= 0; i<numberOfSimultaneousDownloads; i++)  
        {  
            ExeRunner runner = runners[i];  
            Console.WriteLine("\nyoutubedl({0})\n- - - - - - - -\nResultados:\n{1}\nErrores:\n{2}", videos[i][0], runner.Output, runner.Exception);
        } 


        //Probando clase con otro ejecutable

        // ExeRunner youtubeDl = new ExeRunner("RA2MD.exe", "C:\\Program Files (x86)\\Command And Conquer Red Alert 2\\", "C:\\Program Files (x86)\\Command And Conquer Red Alert 2\\");

        // List<string[]> parametros = new List<string[]>();
        // // parametros.Add(new string[]{"--ffmpeg-location", "\"" + ExeRunner.StartupPath +"ffmpeg.exe" + "\"" });
        // // parametros.Add(new string[]{"--extract-audio"});
        // // parametros.Add(new string[]{"--audio-format","mp3"});
        // // parametros.Add(new string[]{"--no-playlist"}); 
        // // parametros.Add(new string[]{"https://www.youtube.com/watch?v=bpOSxM0rNPM"});

        // youtubeDl.Parameters = parametros;
        // youtubeDl.Execute();
    }


}