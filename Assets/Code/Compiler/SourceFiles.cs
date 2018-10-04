using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

namespace BotArena { 
  public class SourceFiles {

    public static readonly string PATH = DataManager.ROBOTS_PATH;

    public static string[] FileNames(){
      new System.IO.FileInfo(PATH).Directory.Create();
      return Directory.GetFiles(PATH, "*.cs", SearchOption.TopDirectoryOnly)
        .Select(path => path.Substring(PATH.Length, path.Length - PATH.Length))
        .ToArray();;
    }

    public static void Save(string filename, string sourceCode){
      string fullPath = Path.Combine(PATH, filename);
      new System.IO.FileInfo(PATH).Directory.Create();
      File.WriteAllText(fullPath, sourceCode);
    }

    public static string Open(string filename){
      return File.ReadAllText(Path.Combine(PATH, filename));
    }

  }
}