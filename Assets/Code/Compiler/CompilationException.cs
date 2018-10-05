using System;
using System.Runtime;

namespace BotArena {
public class CompilationException : Exception{
  private string sourceCode;
  public CompilationException() { }
  public CompilationException(string message) : base(message) { }
  public CompilationException(string message, string sourceCode) : base($"{message}\nSource code:\n\n{sourceCode}") {  }
  public CompilationException(string message, Exception inner) : base(message, inner) { }
  protected CompilationException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
}