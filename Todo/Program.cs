// For making the ui!!
// ╭──┬──╮
// │  │  │
// ├──┼──┤
// │  │  │
// ╰──┴──╯

Todo todo = new Todo();

try
{
    todo.ReadFile();
}
catch
{ // Maybe I should just make the file without asking for input, makes it more smooth
    Console.Clear();
    Console.Write(".vumc_todo.txt is not found. Create one? (Y/N):\n  > ");
    if (Console.ReadKey().Key == ConsoleKey.Y)
    {
        Directory.CreateDirectory(todo.pathConfig);
        todo.WriteFile();
        Console.WriteLine($"\n\nFile created at: {todo.pathConfig}");
        todo.ContinueReadKey();
    }
    else
    {
        Console.WriteLine("fwaaaah");
        Console.ReadKey();
        Environment.Exit(0);
    }
}

while (true)
{
    Console.Clear();
    // Maybe I could add a big title with figlet or smth
    Console.WriteLine("╭───┬──────────────────────────────────────────╮");
    Console.WriteLine("│ 1 │ Add new entry                            │");
    Console.WriteLine("│ 2 │ List all entries                         │");
    Console.WriteLine("│ 3 │ Move entry to different list             │");
    Console.WriteLine("│ 4 │ Trash entry                              │");
    Console.WriteLine("│ 5 │ Edit Entry                               │");
    Console.WriteLine("│ 6 │ Settings                                 │");
    Console.WriteLine("│ 7 │ Exit                                     │");
    Console.WriteLine("╰───┴──────────────────────────────────────────╯");
    Console.Write("Select option:\n  > ");

    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.D1: // Add new entry
            todo.AddEntry();
            break;
        case ConsoleKey.D2: // List all entries
            todo.ListEntry();
            break;
        case ConsoleKey.D3: // Move entry to different list
            todo.MoveEntry();
            break;
        case ConsoleKey.D4: // Delete entry
            todo.DeleteEntry();
            break;
        case ConsoleKey.D5: // Edit entry
            todo.EditEntry();
            break;
        case ConsoleKey.D6: // Config
            todo.Config();
            break;
        case ConsoleKey.D7: // Exit
            Environment.Exit(0);
            break;
        default:
            break;
    }
}

struct Entry
{
    public string Content;
    public string Urgency;
    public string DateTime;

    public Entry(string content, string urgency, string dateTime)
    {
        Content = content;
        Urgency = urgency;
        DateTime = dateTime;
    }
}

class Todo
{
    List<Entry> listTodo = new List<Entry> ();
    List<Entry> listDoing = new List<Entry> ();
    List<Entry> listFinished = new List<Entry> ();
    List<Entry> listTrash = new List<Entry> ();

    public string pathConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vumacc/");
    public string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vumacc/todo.txt");

    private void listList(List<Entry> list)
    {
        foreach (Entry i in list)
        {
            Console.WriteLine($"│ {list.IndexOf(i)+1} │ {i.Urgency} {i.Content}");
        }
    }
    private void MoveEntryTo(List<Entry> origin, int destination, bool trashEntry = false)
    {
        Console.WriteLine("╭──────────────────────────────────────────────────────────────────╶─╶╶╶");
        listList(origin);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────╶─╶╶╶");

        string selectText = trashEntry ?
        $"Choose index of entry you want to delete. Select 0 to trash all.\nSelect entry:\n  > "
        :
        $"Choose index of entry you want to move to list {destination}. Select 0 to move all\nSelect entry:\n  > ";

        Console.Write(selectText);
        int entryIndex = Convert.ToInt32(Console.ReadLine());
        entryIndex--; // Decrement to get true index since displayed index is +1

        if (entryIndex >= origin.Count)
        {
            Console.WriteLine("Index entered is higher than list count");
            Environment.Exit(0);
        }

        if (entryIndex == -1)
        {
            foreach (Entry i in origin)
            {
                MoveEntryTo2(origin, destination, i.Content, origin.IndexOf(i));
            }
            origin.Clear();
        }
        else
        {
            string entryToMove = origin[entryIndex].Content;
            MoveEntryTo2(origin, destination, entryToMove, entryIndex);
            origin.RemoveAt(entryIndex);
        }

        string movedText = trashEntry ?
        $"\nTrashed entry {entryIndex}"
        :
        $"\nMoved entry {entryIndex+1} to list {destination}";
        // string moveAllText = (entryIndex == -1) ?
        // $"\nTrashed all entries"
        // :
        // $"\nMoved all entries to list {destination}";

        Console.WriteLine(movedText);

        ContinueReadKey();
        WriteFile();
    }
    private void MoveEntryTo2(List<Entry> origin, int destination, string moveContent, int index)
    {
        Entry entry = new Entry(moveContent, origin[index].Urgency, Convert.ToString(DateTime.Now));
        switch (destination)
        {
            case 1: // To-Do
                listTodo.Add(entry);
                break;
            case 2: // Doing
                listDoing.Add(entry);
                break;
            case 3: // Finished
                listFinished.Add(entry);
                break;
            case 4: // Trash
                listTrash.Add(entry);
                break;
        }
    }
    private void EditEntryContent(List<Entry> listCopy, int listId)
    {
        Console.Clear();
        Console.WriteLine("╭──────────────────────────────────────────────────────────────────╶─╶╶╶");
        listList(listCopy);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────╶─╶╶╶");
        Console.Write("Select index of entry you want to change the content of:\n  > ");

        try
        {
            int entryIndex = Convert.ToInt32(Console.ReadLine());
            entryIndex--; // Decrementing for same reason in MoveEntryTo()

            Console.Clear();
            Console.WriteLine("Current entry content:");
            Console.WriteLine($"  \"{listCopy[entryIndex].Content}\"");
            Console.Write("\nReplace content with:\n  > ");
            string newContent = Console.ReadLine();

            switch (listId)
            {
                case 1: // To-Do
                    Entry entryTodo = listTodo[entryIndex];
                    entryTodo.Content = newContent;
                    listTodo[entryIndex] = entryTodo;
                    break;
                case 2: // Doing
                    Entry entryDoing = listDoing[entryIndex];
                    entryDoing.Content = newContent;
                    listDoing[entryIndex] = entryDoing;
                    break;
                case 3: // Finished
                    Entry entryFinished = listFinished[entryIndex];
                    entryFinished.Content = newContent;
                    listFinished[entryIndex] = entryFinished;
                    break;
                default:
                    throw new Exception("listId out of range");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("EditEntryContent() exception\n\n"+e);
            ContinueReadKey();
        }
    }
    private void EditEntryUrgency(List<Entry> listCopy, int listId)
    {
        Console.Clear();
        Console.WriteLine("╭──────────────────────────────────────────────────────────────────╶─╶╶╶");
        listList(listCopy);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────╶─╶╶╶");
        Console.Write("Select index of entry you want to change the urgency of:\n  > ");

        try
        {
            int entryIndex = Convert.ToInt32(Console.ReadLine());
            entryIndex--; // Decrementing for same reason in MoveEntryTo()

            Console.Clear();
            Console.WriteLine("Current entry urgency: "+listCopy[entryIndex].Urgency);
            Console.WriteLine($"╭───┬───────────────────╮");
            Console.WriteLine($"│ID │   Urgency level   │");
            Console.WriteLine($"├───┼───────────────────┤");
            Console.WriteLine($"│ 1 │ {Urgency(1)} Immediate       │");
            Console.WriteLine($"│ 2 │ {Urgency(2)} Emergency       │");
            Console.WriteLine($"│ 3 │ {Urgency(3)} Urgent          │");
            Console.WriteLine($"│ 4 │ {Urgency(4)} Semi-urgent     │");
            Console.WriteLine($"│ 5 │ {Urgency(5)} Non-urgent      │");
            Console.WriteLine($"╰───┴───────────────────╯");
            Console.WriteLine("Choose ID of new urgency level");
            Console.Write("Replace content with:\n  > ");
            int newUrgency = Convert.ToInt32(Console.ReadLine());

            switch (listId)
            {
                case 1: // To-Do
                    Entry entryTodo = listTodo[entryIndex];
                    entryTodo.Urgency = Urgency(newUrgency);
                    listTodo[entryIndex] = entryTodo;
                    break;
                case 2: // Doing
                    Entry entryDoing = listDoing[entryIndex];
                    entryDoing.Urgency = Urgency(newUrgency);
                    listDoing[entryIndex] = entryDoing;
                    break;
                case 3: // Finished
                    Entry entryFinished = listFinished[entryIndex];
                    entryFinished.Urgency = Urgency(newUrgency);
                    listFinished[entryIndex] = entryFinished;
                    break;
                default:
                    throw new Exception("listId out of range");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("EditEntryUrgency() exception\n\n"+e);
            ContinueReadKey();
        }
    }
    private string Urgency(int urgency)
    {
        switch (urgency)
        {
            case 1: // Immediate
                return "\e[41m \e[0m";  // Colour red
            case 2: // Emergency:
                return "\e[43m \e[0m";  // Colour yellow
            case 3: // Urgent
                return "\e[42m \e[0m";  // Colour green
            case 4: // Semi-urgent
                return "\e[100m \e[0m"; // Colour grey
            case 5: // Non-urgent
                return "\e[47m \e[0m";  // Colour white

            default:
                return "defaukt";
        }
    }
    public void AddEntry()
    {
        Console.Clear();
        Console.Write("Add new entry to To-Do list:\n  > ");
        string content = Console.ReadLine();

        Console.WriteLine();
        Console.WriteLine($"╭───────────────────────╮");
        Console.WriteLine($"│ID |   Urgency level   │");
        Console.WriteLine($"├───────────────────────┤");
        Console.WriteLine($"│ 1 | {Urgency(1)} Immediate       │");
        Console.WriteLine($"│ 2 | {Urgency(2)} Emergency       │");
        Console.WriteLine($"│ 3 | {Urgency(3)} Urgent          │");
        Console.WriteLine($"│ 4 | {Urgency(4)} Semi-urgent     │");
        Console.WriteLine($"│ 5 | {Urgency(5)} Non-urgent      │");
        Console.WriteLine($"╰───────────────────────╯");
        Console.Write("Choose ID of urgency you want to assign to the entry. Default = 5.\nSelect ID:\n  > ");

        int urgency = 5;
        try {
            urgency = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            // Urgency will be the default
        }
        Entry entry = new Entry(content, Urgency(urgency), Convert.ToString(DateTime.Now));

        listTodo.Add(entry);
        WriteFile();
    }
    public void ListEntry()
    {
        Console.Clear();
        Console.WriteLine("╭───── To-Do ──────────────────────────────────────────────────────────────╶─╶╶╶");
        listList(listTodo);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────────────╶─╶╶╶");

        Console.WriteLine();
        Console.WriteLine("╭───── Doing ──────────────────────────────────────────────────────────────╶─╶╶╶");
        listList(listDoing);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────────────╶─╶╶╶");

        Console.WriteLine();
        Console.WriteLine("╭───── Finished ───────────────────────────────────────────────────────────╶─╶╶╶");
        listList(listFinished);
        Console.WriteLine("╰──────────────────────────────────────────────────────────────────────────╶─╶╶╶");
        ContinueReadKey();
    }
    public void MoveEntry()
    {
        Console.Clear();
        Console.WriteLine("╭───┬────────────────────╮");
        Console.WriteLine("│ID │     List Name      │");
        Console.WriteLine("├───┼────────────────────┤");
        Console.WriteLine("│ 1 │ To-Do              │");
        Console.WriteLine("│ 2 │ Doing              │");
        Console.WriteLine("│ 3 │ Finished           │");
        Console.WriteLine("╰───┴────────────────────╯");

        Console.WriteLine("Origin is where an entry will be taken from. Destination is where it will be moved to.");
        Console.WriteLine("Select list ID of origin and destination (> {origin} to {destination}):");
        Console.Write("  > ");

        try
        {
            string rawMoveInput = Console.ReadLine();
            string[] moveInput = rawMoveInput.Split(" ");

            int origin = Convert.ToInt32(moveInput[0]);
            int destination = Convert.ToInt32(moveInput[2]);

            Console.Clear();
            if (origin == 1) // To-Do
            {
                MoveEntryTo(listTodo, destination);
            }
            else if (origin == 2) // Doing
            {
                MoveEntryTo(listDoing, destination);
            }
            else if (origin == 3) // Finsished
            {
                MoveEntryTo(listFinished, destination);
            }
            else
            {
                Console.WriteLine("\nMoveEntry origin out of range");
                Environment.Exit(0);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\nMoveEntry Exception\n\n"+e);
            ContinueReadKey();
        }
    }
    public void DeleteEntry()
    {
        Console.Clear();
        Console.WriteLine("╭───┬────────────────────╮");
        Console.WriteLine("│ID │     List Name      │");
        Console.WriteLine("├───┼────────────────────┤");
        Console.WriteLine("│ 1 │ To-Do              │");
        Console.WriteLine("│ 2 │ Doing              │");
        Console.WriteLine("│ 3 │ Finished           │");
        Console.WriteLine("│ . ├────────────────────┤");
        Console.WriteLine("│ . │      Actions       │");
        Console.WriteLine("│ . ├────────────────────┤");
        Console.WriteLine("│ 4 │ View trash list    │");
        Console.WriteLine("│ 5 │ Clear trash list   │");
        Console.WriteLine("╰───┴────────────────────╯");

        Console.WriteLine("\nSelect list ID of where you want to trash an entry, or action you would like to do:");
        Console.Write("  > ");
        try
        {
            int trashInput = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            switch (trashInput)
            {
                case 1: // Trash entry in To-Do
                    MoveEntryTo(listTodo, 4, true);
                    break;
                case 2: // Trash entry in Doing
                    MoveEntryTo(listDoing, 4, true);
                    break;
                case 3: // Trash entry in Finished
                    MoveEntryTo(listFinished, 4, true);
                    break;
                case 4: // View Trash list
                    Console.Clear();
                    Console.WriteLine("Trash list is not saved unlike other lists");
                    Console.WriteLine("╭──────────────────────────────────────────────────────────────────╶─╶╶╶");
                    listList(listTrash);
                    Console.WriteLine("╰──────────────────────────────────────────────────────────────────╶─╶╶╶");
                    ContinueReadKey();
                    break;
                case 5: // Clear Trash list
                    listTrash.Clear();
                    break;
                default:
                    Console.WriteLine("DeleteEntry trashInput out of range");
                    Environment.Exit(0);
                    break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("\nUnhandled :PPP");
            ContinueReadKey();
        }
    }
    public void EditEntry()
    {
        Console.Clear();
        Console.WriteLine("╭───┬──────────────────────────────╮");
        Console.WriteLine("│ID │         List Name            │");
        Console.WriteLine("├───┼──────────────────────────────┤");
        Console.WriteLine("│ 1 │ To-Do                        │");
        Console.WriteLine("│ 2 │ Doing                        │");
        Console.WriteLine("│ 3 │ Finished                     │");
        Console.WriteLine("╰───┴──────────────────────────────╯");
        Console.WriteLine("╭───┬──────────────────────────────╮");
        Console.WriteLine("│ID │          Actions             │");
        Console.WriteLine("├───┼──────────────────────────────┤");
        Console.WriteLine("│ 1 │ Change content of an entry   │");
        Console.WriteLine("│ 2 │ Change urgency of an entry   │");
        Console.WriteLine("╰───┴──────────────────────────────╯");

        Console.WriteLine("Choose ID of list and the ID of the action you would like to do it");
        Console.WriteLine("(> {ListID} {ActionID})");
        Console.WriteLine("Select list ID and action ID:");
        Console.Write("  > ");

        try
        {
            string rawEditInput = Console.ReadLine();
            string[] EditInput = rawEditInput.Split(" ");

            int subject = Convert.ToInt32(EditInput[0]);
            int action = Convert.ToInt32(EditInput[1]);

            List<Entry> subjectList;

            switch (subject)
            {
                case 1:
                    subjectList = listTodo;
                    break;
                case 2:
                    subjectList = listDoing;
                    break;
                case 3:
                    subjectList = listFinished;
                    break;
                default:
                    throw new Exception("EditEntry() subject out of range");
            }

            switch (action)
            {
                case 1:
                    EditEntryContent(subjectList, subject);
                    break;
                case 2:
                    EditEntryUrgency(subjectList, subject);
                    break;
                default:
                    throw new Exception("EditEntry() action out of range");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\nEditEntry Exception\n\n"+e);
            ContinueReadKey();
        }
    }
    public void Config()
    {
        Console.Clear();
        Console.WriteLine("Hi my name is Verity");
        ContinueReadKey();
    }
    public void WriteFile()
    {
        try
        {
            StreamWriter writer = new StreamWriter(path);

            using (writer)
            {
                foreach (Entry i in listTodo)
                {
                    writer.WriteLine($"todo:::{i.Content}:::{i.Urgency}:::{i.DateTime}");
                }
                foreach (Entry i in listDoing)
                {
                    writer.WriteLine($"doing:::{i.Content}:::{i.Urgency}:::{i.DateTime}");
                }
                foreach (Entry i in listFinished)
                {
                    writer.WriteLine($"finish:::{i.Content}:::{i.Urgency}:::{i.DateTime}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nwirte fail {path}\n\n{e}");
            ContinueReadKey();
        }
    }
    public void ReadFile()
    {
        StreamReader reader = new StreamReader(path);

        using (reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lineParse = line.Split(":::");
                Entry entry = new Entry(
                    lineParse[1],
                    lineParse[2],
                    lineParse[3]
                );

                if (lineParse[0] == "todo")
                {
                    listTodo.Add(entry);
                }
                else if (lineParse[0] == "doing")
                {
                    listDoing.Add(entry);
                }
                else if (lineParse[0] == "finish")
                {
                    listFinished.Add(entry);
                }
            }
        }
    }
    public void ContinueReadKey()
    {
        Console.Write("\nPress any key to continue...");
        Console.ReadKey();
    }
}
