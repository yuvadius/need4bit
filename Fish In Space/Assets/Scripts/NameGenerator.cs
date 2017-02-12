using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameGenerator : MonoBehaviour
{
    static string path = "Assets/BotNames/names.txt";
    static int numOfLines = GetNumberOfLines(path);
    static string[] lines = GetNames();

    static int GetNumberOfLines(string path)
    {
        int nLines = 0;
        using (var reader = new StreamReader(File.Open(path, FileMode.Open)))
        {
            nLines = reader.ReadToEnd().Split('\n').Length;
        }
        return nLines;
    }

    static string[] GetNames()
    {
        string[] lines = new string[numOfLines];
        using (var reader = new StreamReader(File.Open(path, FileMode.Open)))
        {
            for (int i = 0; i < numOfLines; i++)
            {
                lines[i] = reader.ReadLine();
                //print(lines[i]);
            }
        }
        return lines;
    }

    public static string GetRandomName()
    {
        int myRandom = Random.Range(0, numOfLines);
        return lines[myRandom];
    }
}
