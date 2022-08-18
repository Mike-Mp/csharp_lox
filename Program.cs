class Lox
{

    static Boolean hadError = false;

    private static void Main(String[] args)
    {
        if (args.Length < 1)
        {
            runPrompt();
        }
        else if (args.Length == 1)
        {
            runFile(args[1]);
        }
        else if (args.Length > 1)
        {
            Console.WriteLine("Usage: csharpLox [script]");
            System.Environment.Exit(1);
        }
    }

    private static void runPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            String line = Console.ReadLine();
            if (line == null || line == "^Z")
            {
                Console.WriteLine("bye");
                break;
            }
            run(line);
            hadError = false;
        }
    }

    private static void runFile(String path)
    {
        Byte[] bytes = File.ReadAllBytes(path);
        String text = System.Text.Encoding.Default.GetString(bytes);

        run(text);
    }

    private static void run(String source) {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.scanTokens();

        if (hadError) {
            System.Environment.Exit(1);
        }

        for (int i = 0; i < tokens.Count; i++ ) {
            Console.WriteLine(tokens[i].type + " " + tokens[i].lexeme);
        }
    }

    public static void error(int line, String message) {
        report(line, "", message);
    } 

    private static void report(int line, String where, String message) {
        Console.WriteLine("Line: {0} Error: {1} {2}", line, where, message);
        hadError = true;
    }
}