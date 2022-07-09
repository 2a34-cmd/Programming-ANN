using Atomic.ArtificialNeuralNetwork.libraries;
namespace Atomic.ArtificialNeuralNetwork.Driver {
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Neural Network Driver";
            ConfigFile config;
            string? command = "";
            List<string> strings = new(args);
            string[] help = { "welcome to help section!",
                "for every thing between two brackets(), that means it's optional",
                "the main command is the first word",
                "the arguments are parameters for the command, they are written after the main command as" +
                " /argument_type argument_value",
                "if the function needs just one argument, the \"/argemnt_name\" of the command mustn't be written",
                "if the argument is bool value, like true or false," +
                " writing just the name of the arument is like making it true, otherwise, it's consdired false," +
                " and this is how to use boolean arguments",
                "you can also swap arguments if you want",
                "some commands cannot work if there's excuted command before them in the program; they are marked as [first]",
                "there's a command word must be hidden to work:just write its arument and it'll work; it's marked as [hidden]",
                "commands and their arguments:",
                "\t[first][hidden]export /p path:",
                " export networkdic and pathdic from file that its path is in /p argument",
                "\t[first] random (/n num)(/p path):",
                " construct random number or (/n num) of neural networks(max is 20) with random number of layers(max is 10)" +
                " with random number of neurons(max is 10)" +
                " with random biases which is fully connected and additional random number of jumbing connections with random wieghts" +
                "and it saves all the info in file if /p path is determained",
                "\tclear:",
                " clear the console",
                "\texit:",
                " exit the program",
                "\tsave /p path:",
                " save all the recent info in a file with the determained path",
                "\tenter /a nums /n id:",
                " change values of neurons of input layer of network id /n id",
                "\textract /n id:",
                " display values of neurons of output layer of network id /n id",
                "\tcalc /n id (/a nums) (/o):",
                " enter values of /in if there's /a, otherwise, uses the existed values of input layer," +
                " and then calculate the network of id /n id from input layer to output one, and display the results of" +
                " output layer if /o is true(written)",
                "\tfind (/l) (/n id)",
                " find if there any paths from every neuron to output layer. this proccess grows exponentially with more layer"
                +" so you can parallelize the proccess by writing /l. to find paths for speacial network, just write /n id of network"
                +". this proccess is essential for back propagation if not all paths are found",
                "\tbp /n id /i input /e expected (/d change) (/l):",
                " do back propagation algorathim to the network of id /n id by feeding" +
                " /i input one after another and checkig " +
                "/e arrays as expected results from the network and change learning rate according to /d change." +
                " if there's no /d change, then change is aotumatically 0" +
                " this proccess does take time, so to end it faster," +
                " you can parallelize the proccess by writing /l ",
                "\tlog (/n id) (/v or /w or /t):",
                " display the info of networks, network of id /n id, and displays values of neurons if /v is true, " +
                "or displays biases and wieghts if /w is true, or just displays paths of network if /t is on",
                "\thelp"," display what is already displayed😁",
                "arguments' names meanings:",
                "/p file path: value should be text and valid path",
                "/n number: value should be integer",
                "/a array: value should be integers with space between them and ';' in the end (as 2 4 -2;)," +
                " or valid text file path with content like what is described",
                "/i batch or /e expected: value should be array of arrays, and it's written as list of arrays with comma between them" +
                "and ';' in the end (as' 2 4 -2,3 3 -1;') which first array has 2,4,-2 and second one has 3,3,-1, or it can be valid text file path" +
                " with content like what is described",
                "/d decimal: value should be integer or floating point number (as 2  -3  -0.5  0.4223)",
                "/o output bool: boolean argument of displaying output",
                "/l parallel bool: bool argument of parallelization",
                "/t path bool : boolean argument of displaying paths",
                "/v value bool: boolean argument of displaying values",
                "/w weight bool: boolean argument of displaying wieghts and biases"
            };
            if (args.Length == 0)
            {
                Console.WriteLine("Welcome to Neural Network Driver (NND)!");
                Console.WriteLine("To start using the program, provide the file's path of the networks");
                Console.WriteLine("If there's no such file or you don't want to do it, you have alternative option");
                Console.WriteLine("you can randomize a network (or networks)");
                Console.WriteLine("you can either write the file name or \"random (/n num) (/p path)\" for randomize function");
                Console.WriteLine("for browsing more possible commands and thier parameters, you can write \"help\"");

            }
            if (args.Length > 1) { Console.WriteLine("you can't open more than one file in the same porgram!"); }
            bool cannot = true;
            while (cannot)
            {
                if(strings.Count == 1) 
                {
                    config = new(args[0]);
                    config.CreateANN();
                    cannot = false;
                    continue;
                }
                Console.Write(">");
                command = Console.ReadLine();
                switch (command)
                {
                    case "help":
                        for (int i = 0; i < help.Length; i++)
                        {
                            Task.Run(() => Console.WriteLine(help[i])).Wait();
                        }
                        break;
                    case string a when a.StartsWith("random "):
                        if (a.Contains("/n"))
                        {
                            NetworkDic.RandomNetworks(N(a));
                            cannot = false;
                        }
                        else
                        {
                            NetworkDic.RandomNetworks();
                            cannot = false;
                        }
                        if (a.Contains("/p"))
                        {
                            try
                            {
                                config = new(P(a));
                                config.EditFile();
                                cannot = false;
                            }
                            catch(IOException ex)
                            {
                                Console.WriteLine("the path is not valid, or access is denied. details:\n"+ex.Message);
                            }
                        }
                        break;
                    case string a when a.Contains('.'):
                        try
                        {
                            config = new(a.Trim());
                            config.CreateANN();
                            cannot = false;
                        }
                        catch
                        {
                            Console.WriteLine("this is not valid path, or access is denied");
                        }
                        break;
                    default:
                        Console.WriteLine("That is not valid command; write help for more info about valid commands");
                        break;
                }
            }
            while (true)
            {
                Console.Write(">");
                command = Console.ReadLine();
                NeuralNetwork network;
                try
                {
                    switch (command)
                    {
                        case "clear":
                            Console.Clear();
                            break;
                        case string a when a.StartsWith("save"):
                            ConfigFile file = new(P(a));
                            file.EditFile();
                            break;
                        case string a when a.StartsWith("enter"):
                            NetworkDic.Networks[N(a)].Neglect();
                            NetworkDic.Networks[N(a)].EnterVal(A(a));
                            break;
                        case string a when a.StartsWith("extract"):
                            Console.WriteLine(TS(NetworkDic.Networks[N(a)].ExtractVal()));
                            break;
                        case string a when a.StartsWith("calc"):
                            network = NetworkDic.Networks[N(a)];
                            network.Finilize();
                            if (a.Contains("/a")) network.Calculate(A(a));
                            else network.Calculate();
                            if (O(a)) Console.WriteLine(TS(network.ExtractVal()));
                            break;
                        case String a when a.StartsWith("find"):
                            if (a.Contains("/n"))
                            {
                                network = NetworkDic.Networks[N(a)];
                                network.Finilize();
                                if (L(a)) network.FindPathsParallel();
                                else network.FindPaths();
                            }
                            else
                            {
                                if (L(a))
                                {
                                    foreach (var item in NetworkDic.Networks.Values)
                                    {
                                        item.Finilize();
                                        item.FindPathsParallel();
                                    }
                                }
                                else
                                {
                                    foreach (var item in NetworkDic.Networks.Values)
                                    {
                                        item.Finilize();
                                        item.FindPaths();
                                    }
                                }
                            }
                            break;
                        case String a when a.StartsWith("log"):
                            network = NetworkDic.Networks[N(a)];
                            if (T(a))
                            {
                                Console.WriteLine("Paths:");
                                foreach (var item in network.PathList.ToArray())
                                {
                                    Console.WriteLine('\t' + item.Name);
                                }
                            }
                            else
                            {
                                if (V(a))
                                {
                                    network.InfowC();
                                }
                                else if (W(a))
                                {
                                    network.InfowB();
                                }
                                else
                                {
                                    network.InfoLog();
                                }
                            }
                            break;
                        case String a when a.StartsWith("bp"):
                            network = NetworkDic.Networks[N(a)];
                            network.Neglect();
                            if (L(a)) network.BatchProccessParallel(E(a), I(a), a.Contains("/d")? D(a):0);
                            else network.BatchProccess(E(a), I(a), a.Contains("/d")?D(a): 0);
                            break;
                        case "help":
                            for (int i = 0; i < help.Length; i++)
                            {
                                Task.Run(() => Console.WriteLine(help[i])).Wait();
                            }
                            break;
                        default:
                            Console.WriteLine("this is not valid command. check help for valid ones");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("something went wrong, check your command");
                }
            }
        }
        #region letters
        static string P(string inp)
        {
            return inp.Split("/p")[1].Split("/n")[0].Trim();
        }
        static decimal D(string inp)
        {
            string para = inp.Split("/d")[1].Split("/i")[0].Split("/e")[0].Split("/l")[0].Split("/n")[0].Trim();
            _ = decimal.TryParse(para, out decimal val);
            return val;
        }
        static int N(string inp)
        {
            return int.Parse(inp.Split("/n")[1].Split("/p")[0].Split("/i")[0].
                Split("/l")[0].Split("/v")[0].Split("/w")[0].Split("/f")[0].
                Split("/t")[0].Trim());
        }
        static decimal[] A(string inp)
        {
            string t = inp.Split("/a")[1].Split("/o")[0].Split("/n")[0].Trim();
            if (!t.EndsWith(';'))
            {
                Task<string> task= Task.Run(async () => await File.ReadAllTextAsync(t));
                task.Wait();
                t = task.Result;
            }
            return ConfigFile.Extractdnum(t).ToArray();
        }
        static decimal[][] I(string inp)
        {
            string t = inp.Split("/i")[1]
                    .Split("/e")[0].Split("/l")[0].Split("/n")[0].Trim();
            if (!t.EndsWith(';'))
            {
                Task<string> task = Task.Run(async () => await File.ReadAllTextAsync(t));
                task.Wait();
                t = task.Result;
            }
            return DeciamlExtract(t).ToArray();
        }
        static decimal[][] E(string inp)
        {
            string t = inp.Split("/e")[1]
                    .Split("/i")[0].Split("/l")[0].Split("/n")[0].Trim();
            if (!t.EndsWith(';'))
            {
                Task<string> task = Task.Run(async () => await File.ReadAllTextAsync(t));
                task.Wait();
                t = task.Result;
            }
            return DeciamlExtract(t).ToArray();
        }
        static List<decimal[]> DeciamlExtract(string t)
        {
            List<decimal> ints = new();
            List<decimal[]> listofarray = new();
            string parameter = "";
            foreach (char c in t)
            {
                switch (c)
                {
                    case char when char.IsDigit(c):
                        parameter += c;
                        break;
                    case '-':
                        parameter += c;
                        break;
                    case '.':
                        parameter += c;
                        break;
                    case ' ':
                        ints.Add(decimal.Parse(parameter));
                        parameter = "";
                        break;
                    case ',':
                        listofarray.Add(ints.ToArray());
                        ints = new();
                        break;
                    case ';':
                        ints.Add(decimal.Parse(parameter));
                        listofarray.Add(ints.ToArray());
                        return listofarray;
                    default:
                        throw new("there's a char not supported");
                }
            }
            throw new("something is wrong, contant the developer");
        }
        static bool O(string inp) { return inp.Contains("/o"); }
        static bool L(string inp) { return inp.Contains("/l"); }
        static bool T(string inp) { return inp.Contains("/t"); }
        static bool V(string inp) { return inp.Contains("/v"); }
        static bool W(string inp) { return inp.Contains("/w"); }
        static bool F(string inp) { return inp.Contains("/f"); }
        #endregion
        static string TS(List<decimal> decimals)
        {
            String returned = "";
            foreach (var item in decimals)
            {
                returned += $"{item} ";
            }
            return returned;
        }
    }
}