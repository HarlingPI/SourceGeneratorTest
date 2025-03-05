using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class NewScript 
{
    static NewScript()
    {
        //Debug.Log(SourceGeneratorInCSharp.HelloWorld.SayHello());

        var test = new TestClass() { Name = "John", Age = 30 };
        var str = test.ToString();
        Debug.Log(str);

        Debug.Log(ExampleSourceGenerated.ExampleSourceGenerated.GetTestText());
    }
}
public class AutoToString : Attribute
{
}
[AutoToString]
public partial class TestClass
{
    public string Name { get; set; }
    public int Age { get; set; }

}
