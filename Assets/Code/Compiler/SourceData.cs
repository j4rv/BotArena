using System;

[Serializable]
public class SourceData {
  public string author;
  public string code;
  
  public SourceData(string code){
    this.author = Environment.UserName;
    this.code = code;
  }

  override public string ToString(){
    return $"/* author: {author} */\n\n{code}";
  }
}