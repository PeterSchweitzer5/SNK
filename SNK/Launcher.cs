using System.Diagnostics;

namespace SNK;

public static class Launcher
{
    public static void Launch(Workspace workspace)
    {
        foreach (string url in workspace.Urls)
        {
            Console.WriteLine(Lang.T("opening_url", url));
            Start(url);
        }

        foreach (string app in workspace.Apps)
        {
            Console.WriteLine(Lang.T("starting_app", app));
            Start(app);
        }
    }

    private static void Start(string target)
    {
        try
        {
            Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine("  " + Lang.T("start_error", target, ex.Message));
        }
    }
}