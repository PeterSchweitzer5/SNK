using SNK;

Settings settings = Settings.Load();
Lang.Load(settings.Language);

string[] reserved = { "list", "new", "add", "remove", "delete", "help", "lang" };

if (args.Length == 0 || args[0] == "help")
{
    PrintHelp();
    return;
}

switch (args[0])
{
    case "lang":
        {
            if (args.Length < 2 || (args[1] != "en" && args[1] != "de"))
            {
                Console.WriteLine(Lang.T("usage_lang"));
                return;
            }
            settings.Language = args[1];
            settings.Save();
            Lang.Load(args[1]);
            Console.WriteLine(Lang.T("lang_set"));
            break;
        }

    case "list":
        {
            if (args.Length >= 2)
            {
                Workspace? workspace = WorkspaceStore.Load(args[1]);
                if (workspace == null)
                {
                    Console.WriteLine(Lang.T("not_found", args[1]));
                    return;
                }

                Console.WriteLine($"Workspace \"{args[1]}\":");
                Console.WriteLine();

                Console.WriteLine("URLs:");
                if (workspace.Urls.Count == 0)
                {
                    Console.WriteLine("  " + Lang.T("none"));
                }
                foreach (string url in workspace.Urls)
                {
                    Console.WriteLine($"  - {url}");
                }

                Console.WriteLine();

                Console.WriteLine("Apps:");
                if (workspace.Apps.Count == 0)
                {
                    Console.WriteLine("  " + Lang.T("none"));
                }
                foreach (string app in workspace.Apps)
                {
                    Console.WriteLine($"  - {app}");
                }
                return;
            }

            List<string> names = WorkspaceStore.ListNames();
            if (names.Count == 0)
            {
                Console.WriteLine(Lang.T("no_workspaces"));
            }
            foreach (string n in names)
            {
                Console.WriteLine($"- {n}");
            }
            break;
        }

    case "new":
        {
            if (args.Length < 2)
            {
                Console.WriteLine(Lang.T("usage_new"));
                return;
            }
            string name = args[1];
            if (reserved.Contains(name))
            {
                Console.WriteLine(Lang.T("reserved", name));
                return;
            }
            if (WorkspaceStore.Exists(name))
            {
                Console.WriteLine(Lang.T("exists", name));
                return;
            }
            WorkspaceStore.Save(name, new Workspace());
            Console.WriteLine(Lang.T("created", name));
            break;
        }

    case "add":
        {
            if (args.Length < 3)
            {
                Console.WriteLine(Lang.T("usage_add"));
                return;
            }
            Workspace? workspace = WorkspaceStore.Load(args[1]);
            if (workspace == null)
            {
                Console.WriteLine(Lang.T("not_found", args[1]));
                return;
            }
            string entry = args[2];
            if (IsUrl(entry))
            {
                workspace.Urls.Add(entry);
            }
            else
            {
                workspace.Apps.Add(entry);
            }
            WorkspaceStore.Save(args[1], workspace);
            Console.WriteLine(Lang.T("added", entry, args[1]));
            break;
        }

    case "remove":
        {
            if (args.Length < 3)
            {
                Console.WriteLine(Lang.T("usage_remove"));
                return;
            }
            Workspace? workspace = WorkspaceStore.Load(args[1]);
            if (workspace == null)
            {
                Console.WriteLine(Lang.T("not_found", args[1]));
                return;
            }
            string entry = args[2];
            bool removed = workspace.Urls.Remove(entry) || workspace.Apps.Remove(entry);
            if (!removed)
            {
                Console.WriteLine(Lang.T("not_in", entry, args[1]));
                return;
            }
            WorkspaceStore.Save(args[1], workspace);
            Console.WriteLine(Lang.T("removed", entry, args[1]));
            break;
        }

    case "delete":
        {
            if (args.Length < 2)
            {
                Console.WriteLine(Lang.T("usage_delete"));
                return;
            }
            if (!WorkspaceStore.Exists(args[1]))
            {
                Console.WriteLine(Lang.T("not_found", args[1]));
                return;
            }
            WorkspaceStore.Delete(args[1]);
            Console.WriteLine(Lang.T("deleted", args[1]));
            break;
        }

    default:
        {
            Workspace? workspace = WorkspaceStore.Load(args[0]);
            if (workspace == null)
            {
                Console.WriteLine(Lang.T("not_found_hint", args[0]));
                return;
            }
            Launcher.Launch(workspace);
            break;
        }
}

static bool IsUrl(string text) => text.StartsWith("http://") || text.StartsWith("https://");

static void PrintHelp()
{
    Console.WriteLine("SNK");
    Console.WriteLine();
    PrintHelpLine("snk <name>", "help_open");
    PrintHelpLine("snk list", "help_list");
    PrintHelpLine("snk list <name>", "help_list_name");
    PrintHelpLine("snk new <name>", "help_new");
    PrintHelpLine("snk add <name> <entry>", "help_add");
    PrintHelpLine("snk remove <name> <entry>", "help_remove");
    PrintHelpLine("snk delete <name>", "help_delete");
    PrintHelpLine("snk lang <en|de>", "help_lang");
    PrintHelpLine("snk help", "help_help");
}

static void PrintHelpLine(string command, string key)
{
    Console.WriteLine($"  {command,-28} {Lang.T(key)}");
}