using UnityEngine;
using System;
using System.Reflection;

public class CommandListener : MonoBehaviour
{
    void FixedUpdate()
    {
        var DLL = Assembly.LoadFile(@"c:\users\main\documents\visual studio 2015\Projects\ClassLibrary1\ClassLibrary1\bin\Debug\ClassLibrary1.dll");

        foreach (Type type in DLL.GetExportedTypes())
        {
            Debug.Log(type == typeof(IRobot));
            var c = Activator.CreateInstance(type);
            string res = (string)type.InvokeMember("think", BindingFlags.InvokeMethod, null, c, new object[] { transform.position, transform.rotation.eulerAngles });
            Debug.Log(res);
        }
    }
}
