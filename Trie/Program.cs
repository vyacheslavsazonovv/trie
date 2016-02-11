using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class Operations
{
    Trie Tree = new Trie();

    public Boolean loadDictionary(String dicFieName)
    {
        try
        {
            foreach (string line in File.ReadLines(dicFieName))
            {
                Tree.Add(line);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }        
    }

    public List<string> arrCounts = new List<string>();

    public Boolean loadInput(String inputName)
    {
        try
        {
            foreach (string line in File.ReadLines(inputName))
            {
                String[] arrLine = line.Split(',');
                arrCounts.Add(findWords(arrLine[0], Int32.Parse(arrLine[1])).ToString());
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    int findWords(String prefix, int maxLength)
    {
        List<string> foundWords = Tree.Match(prefix, maxLength);
        return foundWords.Count();
    }

    public Boolean writeOutput(String outputName, List<string> listCount)
    {
        listCount.ToArray();
        try
        {
            File.WriteAllLines(outputName, listCount);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
}

public class Trie
{
    public Node RootNode { get; private set; }

    public Trie()
    {
        RootNode = new Node { Letter = Node.Root };
    }

    public void Add(string word)
    {
        word = word.ToLower() + Node.Eow;
        var currentNode = RootNode;
        foreach (var c in word)
        {
            currentNode = currentNode.AddChild(c);
        }
    }

    public List<string> Match(string prefix, int maxLenght)
    {
        prefix = prefix.ToLower();
        var set = new HashSet<string>();
        matchSearch(RootNode, set, "", prefix, maxLenght);
        return set.ToList();
    }

    private static void matchSearch(Node node, ISet<string> rtn, string letters, string prefix, int maxLenght)
    {
        if (node == null)
        {
            if (!rtn.Contains(letters) && letters.Count() <= maxLenght)
            {
                rtn.Add(letters);
            }
            return;
        }
        if (letters == " ")
        {
            letters = node.Letter.ToString();
        }
        else {
            letters += node.Letter.ToString();
        }

        if (prefix.Length > 0)
        {
            if (node.ContainsKey(prefix[0]))
            {
                matchSearch(node[prefix[0]], rtn, letters, prefix.Remove(0, 1), maxLenght);
            }
        }
        else {
            foreach (char key in node.Keys)
            {
                matchSearch(node[key], rtn, letters, prefix, maxLenght);
            }
        }
    }
}

public class Node
{
    public const char Eow = '$';
    public const char Root = ' ';

    public char Letter { get; set; }
    public HybridDictionary Children { get; private set; }

    public Node() { }

    public Node(char letter)
    {
        this.Letter = letter;
    }

    public Node this[char index]
    {
        get { return (Node)Children[index]; }
    }

    public ICollection Keys
    {
        get { return Children.Keys; }
    }

    public bool ContainsKey(char key)
    {
        return Children.Contains(key);
    }

    public Node AddChild(char letter)
    {
        if (Children == null)
        {
            Children = new HybridDictionary();
        }
        if (!Children.Contains(letter))
        {
            var node = letter != Eow ? new Node(letter) : null;
            Children.Add(letter, node);
            return node;
        }
        return (Node)Children[letter];
    }

    public override string ToString()
    {
        return this.Letter.ToString();
    }
}

class Program
{
    static void Main()
    {
        Operations Operations = new Operations();
        if (Operations.loadDictionary("dictionary.txt"))
        {
            Console.WriteLine("Dictionary Loaded!");
            if (Operations.loadInput("input.txt"))
            {
                if (Operations.writeOutput("output.txt", Operations.arrCounts))
                {
                    Console.WriteLine("Done!");
                };
            };

        };
        Console.ReadLine();
    }
}

