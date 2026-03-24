#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class OpenGitBash
{
    [MenuItem("Assets/Open Git Bash Here", false, 999)]
    [MenuItem("Tools/Open Git Bash Here %g", false)]
    private static void LaunchGitBash()
    {
        string projectRootPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string bashPath = ResolveGitBash();

        if (bashPath == null)
        {
            UnityEngine.Debug.LogError("Git Bash could not be resolved. Ensure Git is installed and added to PATH.");
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = bashPath,
            WorkingDirectory = projectRootPath,
            UseShellExecute = true
        });
    }

    private static string ResolveGitBash()
    {
        try
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = "git",
                    Arguments = "--exec-path",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();
                string execPath = process.StandardOutput.ReadLine();
                process.WaitForExit();

                if (string.IsNullOrEmpty(execPath))
                    return null;

                string gitRoot = Directory.GetParent(execPath)
                                       .Parent
                                       .Parent
                                       .FullName;

                string bashPath = Path.Combine(gitRoot, "git-bash.exe");

                return File.Exists(bashPath) ? bashPath : null;
            }
        }
        catch
        {
            return null;
        }
    }
}
#endif