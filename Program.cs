using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {

        ExeRunner youtubeDl = new ExeRunner("youtube-dl.exe", "C:\\Users\\gian\\Music");
        List<string[]> parametros = new List<string[]>();
        parametros.Add(new string[]{"--ffmpeg-location", "\"" + ExeRunner.StartupPath +"ffmpeg.exe" + "\"" });
        parametros.Add(new string[]{"--extract-audio"});
        parametros.Add(new string[]{"--audio-format","mp3"});
        parametros.Add(new string[]{"--no-playlist"}); 
        parametros.Add(new string[]{"https://www.youtube.com/watch?v=bpOSxM0rNPM"});

        youtubeDl.Parameters = parametros;
        youtubeDl.Execute();

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