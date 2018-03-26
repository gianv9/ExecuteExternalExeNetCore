using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        //LaunchCommandLineApp();
        ExeRunner youtubeDl = new ExeRunner("youtube-dl.exe", "C:\\Users\\gian\\Music");

        List<string[]> parametros = new List<string[]>();
        parametros.Add(new string[]{"--ffmpeg-location", "\"" + youtubeDl.WorkingDirectory+"ffmpeg.exe" + "\"" });
        parametros.Add(new string[]{"--extract-audio"});
        parametros.Add(new string[]{"--audio-format","mp3"});
        parametros.Add(new string[]{"--no-playlist"}); 
        parametros.Add(new string[]{"https://www.youtube.com/watch?v=bpOSxM0rNPM"});

        youtubeDl.Parameters = parametros;
        youtubeDl.Execute();
    }


}