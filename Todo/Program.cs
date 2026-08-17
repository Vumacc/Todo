Todo todo = new Todo();

while (true)
{
    Console.Clear();
    Console.WriteLine("    To-Do    ");
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("| 1 | Add new entry                            |");
    Console.WriteLine("| 2 | List all entries                         |");
    Console.WriteLine("| 3 | Move entry to different list             |");
    Console.WriteLine("| 4 | Delete entry                             |");
    Console.WriteLine("| 5 | Mystery option                           |");
    Console.WriteLine("------------------------------------------------");
    Console.Write("Select option:\n  > ");

    int input = Convert.ToInt32(Console.ReadLine());

    switch (input)
    {
        case 1: // Add new entry
            todo.AddEntry();
            break;
        case 2: // List all entries
            todo.ListEntry();
            break;
        case 3: // Move entry to different list
            todo.MoveEntry();
            break;
        case 4: // Delete entry
            todo.DeleteEntry();
            break;
        default:
            Environment.Exit(0);
            break;
    }
}

class Todo
{
    List<string> listTodo = new List<string> ();
    List<string> listDoing = new List<string> ();
    List<string> listFinished = new List<string> ();

    private void listList(List<string> list)
    {
        foreach(string i in list)
        { // Index has +1 so it's easier for people to read when displayed
            Console.WriteLine($"| {(list.IndexOf(i))+1} | {i}");
        }
    }
    private void MoveEntryTo(List<string> origin, int destination)
    {
        Console.WriteLine("------------------------------------------------");
        listList(origin);
        Console.WriteLine("------------------------------------------------");

        Console.Write($"Select index of entry you want to move to list {destination}:\n  > ");
        int entryIndex = Convert.ToInt32(Console.ReadLine());
        entryIndex--; // Decrement to get true index since displayed index is +1

        if (entryIndex >= origin.Count)
        {
            Console.WriteLine("Index entered is higher than list count");
            Environment.Exit(0);
        }

        string entryToMove = origin[entryIndex];

        origin.RemoveAt(entryIndex);
        switch (destination)
        {
            case 1: // To-Do
                listTodo.Add(entryToMove);
                break;
            case 2: // Doing
                listDoing.Add(entryToMove);
                break;
            case 3: // Finished
                listFinished.Add(entryToMove);
                break;
        }

        Console.WriteLine($"\nMoved entry {entryIndex+1} to list {destination}");
    }
    public void AddEntry()
    {
        Console.Clear();
        Console.WriteLine("Add new entry to To-Do list:");
        Console.Write("  > ");
        string newEntry = Console.ReadLine();
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

        string rawMoveInput = Console.ReadLine();
        string[] moveInput = rawMoveInput.Split(" ");

        if (moveInput.Count() >= 4)
        {

        }

        int origin = Convert.ToInt32(moveInput[0]);
        int destination = Convert.ToInt32(moveInput[2]);

        if (origin == destination)
        {
            Console.WriteLine("MoveEntry origin and destination same value");
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

    }
    public void WriteFile()
    {
        // Write .txt file with data
    }
    public void ReadFile()
    {
        // Read .txt file with data
    }
}