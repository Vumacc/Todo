Todo todo = new Todo();

try
{
    todo.ReadFile();
}
catch (Exception)
{ // Maybe I should just make the file without asking for input, makes it more smooth
    Console.Clear();
    Console.Write(".vumc_todo.txt is not found. Create one? (Y/N):\n  > ");
    if (Console.ReadKey().Key == ConsoleKey.Y)
    {
        todo.WriteFile();
    }
    else
    {
        Console.WriteLine("fwaaaah");
        Console.ReadKey();
    }
}

while (true)
{

    Console.Clear();
    // Maybe I could add a big title with figlet or smth
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("| 1 | Add new entry                            |");
    Console.WriteLine("| 2 | List all entries                         |");
    Console.WriteLine("| 3 | Move entry to different list             |");
    Console.WriteLine("| 4 | Trash entry                              |");
    Console.WriteLine("| 5 | Exit                                     |");
    Console.WriteLine("------------------------------------------------");
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
        case ConsoleKey.D5: // Exit
            Environment.Exit(0);
            break;
        case ConsoleKey.D6: // Debug write
            todo.WriteFile();
            break;
        case ConsoleKey.D7: // Debug read
            todo.ReadFile();
            break;
        default:
            break;
    }
}

class Todo
{
    List<string> listTodo = new List<string> ();
    List<string> listDoing = new List<string> ();
    List<string> listFinished = new List<string> ();
    List<string> listTrash = new List<string> ();

    public string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".vumc_todo.txt");

    private void listList(List<string> list)
    {
        foreach(string i in list)
        { // Index has +1 so it's easier for people to read when displayed
            Console.WriteLine($"| {(list.IndexOf(i))+1} | {i}");
        }
    }
    private void MoveEntryTo(List<string> origin, int destination, bool trashEntry = false)
    {
        Console.WriteLine("------------------------------------------------");
        listList(origin);
        Console.WriteLine("------------------------------------------------");

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
            foreach(string i in origin)
            {
                MoveEntryTo2(origin, destination, i);
            }
            origin.Clear();
        }
        else
        {
            string entryToMove = origin[entryIndex];
            origin.RemoveAt(entryIndex);

            MoveEntryTo2(origin, destination, entryToMove);
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
    }
    private void MoveEntryTo2(List<string> origin, int destination, string moveContent)
    {
        switch (destination)
        {
            case 1: // To-Do
                listTodo.Add(moveContent);
                break;
            case 2: // Doing
                listDoing.Add(moveContent);
                break;
            case 3: // Finished
                listFinished.Add(moveContent);
                break;
            case 4: // Trash
                listTrash.Add(moveContent);
                break;
        }
    }
    public void AddEntry()
    {
        Console.Clear();
        Console.WriteLine("Add new entry to To-Do list:");
        Console.Write("  > ");
        string? newEntry = Console.ReadLine();
        listTodo.Add(newEntry);
    }
    public void ListEntry()
    {
        Console.Clear();
        Console.WriteLine("---- To-Do -------------------------------------------------------------\n");
        listList(listTodo);

        Console.WriteLine();
        Console.WriteLine("---- Doing--------------------------------------------------------------\n");
        listList(listDoing);

        Console.WriteLine();
        Console.WriteLine("---- Finished ----------------------------------------------------------\n");
        listList(listFinished);


        Console.Write("\n----------------------------------------------------------------------\n\nPress any key to continue...");
        Console.ReadKey();
    }
    public void MoveEntry()
    {
        Console.Clear();
        Console.WriteLine("--------------------------");
        Console.WriteLine("|ID |     List Name      |");
        Console.WriteLine("--------------------------");
        Console.WriteLine("| 1 | To-Do              |");
        Console.WriteLine("| 2 | Doing              |");
        Console.WriteLine("| 3 | Finished           |");
        Console.WriteLine("--------------------------");

        Console.WriteLine("Origin is where an entry will be taken from. Destination is where it will be moved to.");
        Console.WriteLine("Select list ID of origin and destination ({origin} to {destination}):");
        Console.Write("  > ");

        string? rawMoveInput = Console.ReadLine();
        string[] moveInput = rawMoveInput.Split(" ");

        if (moveInput.Count() >= 4)
        {
            Console.WriteLine("MoveEntry moveInput out of range");
            Environment.Exit(0);
        }

        int origin = Convert.ToInt32(moveInput[0]);
        int destination = Convert.ToInt32(moveInput[2]);

        if (origin == destination)
        {
            Console.WriteLine("MoveEntry origin and destination same value");
            Environment.Exit(0);
        }
        else if (destination >= 4 || origin <= 0)
        {
            Console.WriteLine("MoveEntry destination out of range");
            Environment.Exit(0);
        }

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

        Console.Write("\nPress any key to continue...");
        Console.ReadKey();
    }
    public void DeleteEntry()
    {
                Console.Clear();
        Console.WriteLine("--------------------------");
        Console.WriteLine("|ID |     List Name      |");
        Console.WriteLine("--------------------------");
        Console.WriteLine("| 1 | To-Do              |");
        Console.WriteLine("| 2 | Doing              |");
        Console.WriteLine("| 3 | Finished           |");
        Console.WriteLine("| . |--------------------|");
        Console.WriteLine("| . |      Actions       |");
        Console.WriteLine("| . |--------------------|");
        Console.WriteLine("| 4 | View trash list    |");
        Console.WriteLine("| 5 | Clear trash list   |");
        Console.WriteLine("--------------------------");

        Console.WriteLine("Select list ID of where you want to trash an entry, or action you would like to do:");
        Console.Write("  > ");

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
                Console.WriteLine("------------------------------------------------");
                listList(listTrash);
                Console.WriteLine("------------------------------------------------");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
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
    public void WriteFile()
    {
        StreamWriter writer = new StreamWriter(path);

        using (writer)
        {
            foreach(string i in listTodo)
            {
                writer.WriteLine("todo:"+i);
            }
            foreach(string i in listDoing)
            {
                writer.WriteLine("doing:"+i);
            }
            foreach(string i in listFinished)
            {
                writer.WriteLine("finish:"+i);
            }
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
                string[] lineParse = line.Split(":");

                if (lineParse[0] == "todo")
                {
                    listTodo.Add(lineParse[1]);
                }
                else if (lineParse[0] == "doing")
                {
                    listDoing.Add(lineParse[1]);
                }
                else if (lineParse[0] == "finish")
                {
                    listFinished.Add(lineParse[1]);
                }
            }
        }
    }
}
