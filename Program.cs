using System.Diagnostics;

string sourceDir = string.Empty;
string destinDir = string.Empty;
string sourcePath = string.Empty;
string destinPath = string.Empty;

string album = string.Empty;
string song = string.Empty;

sourceDir = Console.ReadLine().Trim('"');
string[] matches = sourceDir.Split('\\');
album = matches.Last();

destinDir = Path.Combine(@"D:\Music", album);
System.Console.WriteLine($"Source: {sourceDir}");
System.Console.WriteLine($"Destin: {destinDir}");
System.Console.WriteLine();

Directory.CreateDirectory(destinDir);

string[] files = Directory.GetFiles(sourceDir);

using (Process process = new Process())
{
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.FileName = "pwsh";
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardInput = true;

    process.Start();

    using (var sw = process.StandardInput)
    {
        foreach (string file in files)
        {
            sourcePath = file;
            song = sourcePath.Split('\\').Last();
            destinPath = Path.Combine(destinDir, song);
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine($"fsutil hardlink create \"{destinPath}\" \"{sourcePath}\"");
            }
        }
    }

    while (!process.StandardOutput.EndOfStream)
    {
        var line = process.StandardOutput.ReadLine();
        Console.WriteLine(line);
    }
}

System.Console.WriteLine("All Done!");
Console.ReadKey();