using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

public class dialogueVariables : MonoBehaviour
{
    private static dialogueVariables instance = null;

    private Dictionary<string, int> variables = new Dictionary<string, int>();
    
    
    public static dialogueVariables Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int tryGetValue(string s)
    {
        if (variables.ContainsKey(s) == false)
        {
            variables.Add(s, 0);
            return 0;
        }
        return variables[s];
    }

    public void updateValue(string s, int toAdd)
    {
        if (variables.ContainsKey(s) == false)
        {
            variables.Add(s, toAdd);
        }
        else
        {
            variables[s] += toAdd;
        }
    }

    public bool contains(string s) { return variables.ContainsKey(s); }

    public void load()
    {
        List<string> savedData = Resources.Load<TextAsset>("savedText").text.Split('\n').ToList();
        foreach (string s in savedData)
        {
            if (s != '\n'.ToString() && s != "" && s.StartsWith("#") == false)
            {
                string[] KVpair = s.Split(' ');
                string varName = KVpair[0];
                int num = Int32.Parse(KVpair[1]);
                if (variables.ContainsKey(varName) == false)
                {
                    variables.Add(varName, num);
                }
                else
                {
                    variables[varName] = num;
                }
            }
            
        }
    }
    public void save()
    {
        string path = "Assets/Resources/savedText.txt";

        File.WriteAllText(path, string.Empty);

        StreamWriter writer = new StreamWriter(path, true);
        
        foreach(string s in variables.Keys)
        {
            writer.Write(s + " " + variables[s] + '\n');
        }
        writer.Close();

    }
}
