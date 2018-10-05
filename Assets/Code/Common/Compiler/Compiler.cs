using Microsoft.CSharp;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace BotArena { 
public class Compiler<T> where T : class {

  private readonly List<String> errors;
  private readonly TextMeshProUGUI errorsContainer;
  private readonly CSharpCodeProvider csProvider;
  private static readonly T UNDEFINED_T = default(T);
  private readonly string MORE_THAN_ONE_CLASS_IN_SOURCE_ERROR = $"Found more than one class of type {typeof(T)} in source code, this is not allowed";
  private readonly string NO_CLASS_IN_SOURCE_ERROR = $"No exported class of type {typeof(T)} in source code";

  public Compiler() {
    errors = new List<String>();
    this.csProvider = new CSharpCodeProvider();
  }

  public Compiler(TextMeshProUGUI errorsContainer) {
    errors = new List<String>();
    this.errorsContainer = errorsContainer;
    this.csProvider = new CSharpCodeProvider();
  }

  public T CompileAndCreateFromFilename(string robotFilename){
    Debug.Log("Compiling robot " + robotFilename);
    string sourceCode = SourceFiles.Open(robotFilename);
    return CompileAndCreateInstance(sourceCode);
	}

  public T CompileAndCreateInstance(string source){
    errors.Clear();

    var compilerResults = CompileCSharpCode(source);
		T processedResult = ProcessCompilerResults(compilerResults);

    if(processedResult == UNDEFINED_T){
      errors.Add(NO_CLASS_IN_SOURCE_ERROR);
      UpdateErrorsUI();
      throw new CompilationException(NO_CLASS_IN_SOURCE_ERROR, source);
    } else {
      return processedResult;
    }    
	}

  private CompilerResults CompileCSharpCode(string source){
    var compParams = new CompilerParameters{
      GenerateExecutable = false, 
      GenerateInMemory = true
    };
    // This is so that the compiler knows about this project's classes and interfaces
    string[] assemblies = AppDomain.CurrentDomain.GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .Select(a => a.Location)
                            .ToArray();
		compParams.ReferencedAssemblies.AddRange(assemblies);
		return csProvider.CompileAssemblyFromSource(compParams, source);
  }

  private T ProcessCompilerResults(CompilerResults compilerResults){
    T result = UNDEFINED_T;
    if(compilerResults.Errors.HasErrors){
			foreach(CompilerError err in compilerResults.Errors){
				errors.Add($"({err.Line}:{err.Column}) {err.ErrorText}");
			}
		} else {
      try {
        result = CreateInstanceFromCompilerResults(compilerResults);
      } catch (Exception e) {
        errors.Add(e.ToString());
      } finally {
        UpdateErrorsUI();
      }
    }
    return result;
  }

  private T CreateInstanceFromCompilerResults(CompilerResults compilerResults) {
    T instance = UNDEFINED_T;
    foreach (Type type in compilerResults.CompiledAssembly.GetExportedTypes()){
      if(typeof(T).IsAssignableFrom(type)){
        if (instance != UNDEFINED_T) {
          throw new CompilationException(MORE_THAN_ONE_CLASS_IN_SOURCE_ERROR);
        }
        instance = (T) Activator.CreateInstance(type);
      }
    }
    return instance;
  }

  private void UpdateErrorsUI() {
    if(errorsContainer != null){
      errorsContainer.text = String.Join("\n", errors);
    }
  }

}
}