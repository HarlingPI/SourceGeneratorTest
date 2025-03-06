using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SourceGeneratedTest;
using SourceGeneratorInCSharp;


namespace Test
{
    [InitializeOnLoad]
    public class NewScript
    {
        static NewScript()
        {
            Debug.Log(HelloWorld.SayHello());

            var test = new TestClass() { Name = "John", Age = 30 };
            var str = test.ToString();
            Debug.Log(str);

            Debug.Log(ExampleSourceGenerated.GetTestText());
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

}