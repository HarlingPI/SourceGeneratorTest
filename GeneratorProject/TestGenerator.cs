using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorProject
{
    /// <summary>
    /// 作者:   Harling
    /// 时间:   2025/3/5 13:59:26
    /// 备注:   此文件通过PIToolKit模板创建
    /// </summary>
    [Generator]
    public class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var sourceText = """
namespace SourceGeneratorInCSharp
{
    /// <summary>
    /// 作者:   Harling
    /// 时间:   2025/3/5 13:59:26
    /// 备注:   此文件通过PIToolKit模板创建
    /// </summary>
    public static class HelloWorld
    {
        public static void SayHello()
        {
            Console.WriteLine("Hello From Generator");
        }
    }
}
""";
                ctx.AddSource("ExampleGenerator.g", SourceText.From(sourceText, Encoding.UTF8));
            });
        }
    }

}