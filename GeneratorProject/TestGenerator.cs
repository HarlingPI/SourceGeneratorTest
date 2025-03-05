using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace GeneratorProject
{
    /// <summary>
    /// 作者:   Harling
    /// 时间:   2025/3/5 13:59:26
    /// 备注:   此文件通过PIToolKit模板创建
    /// </summary>
    [Generator]
    internal class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //过滤所有的类声明
            var classDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
                static (s, _) => s is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0,
                static (ctx, _) =>
                {
                    var cds = (ClassDeclarationSyntax)ctx.Node;
                    var atr = cds.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "AutoToString");
                    return atr ? cds : null;
                }
                );
            //获取类的信息
            var classinfo = classDeclarations
                .Where(static (cds) => cds != null)
                .Select(static (cds, _) =>
                {
                    var className = cds.Identifier.Text;

                    var namespaceNode = cds.Parent;
                    string? namespaceName = null;
                    while (namespaceNode != null)
                    {
                        if (namespaceNode is NamespaceDeclarationSyntax nds)
                        {
                            namespaceName = nds.Name.ToString();
                            break;
                        }
                        namespaceNode = namespaceNode.Parent;
                    }

                    var properties = cds.Members
                        .OfType<PropertyDeclarationSyntax>()
                        .Select(p => p.Identifier.Text)
                        .ToImmutableArray();
                    return (className, namespaceName, properties);
                });
            //生成代码
            context.RegisterSourceOutput(classinfo, (spc, data) =>
            {
                var (className, namespaceName, properties) = data;

                // ======== 使用容量预设的StringBuilder ========
                var sb = new StringBuilder(1024);

                // ======== 添加编译提示指令 ========
                sb.AppendLine("#nullable enable");
                sb.AppendLine("// <auto-generated />");
                // ======== 命名空间处理 ========
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    sb.AppendLine($"namespace {namespaceName}");
                    sb.AppendLine("{");
                }

                // ======== 使用常量管理缩进 ========
                const string classIndent = "    ";
                sb.AppendLine($"{classIndent}public partial class {className}");
                sb.AppendLine($"{classIndent}{{");

                const string methodIndent = "        ";
                sb.AppendLine($"{methodIndent}public override string ToString()");
                sb.AppendLine($"{methodIndent}{{");

                // ======== 优化后的字符串拼接 ========
                if (properties.Length > 0)
                {
                    sb.Append($"{methodIndent}    return $\"");
                    sb.Append(string.Join(", ", properties.Select(p => $"{p}={{this.{p}}}")));
                    sb.AppendLine("\";");
                }
                else
                {
                    sb.AppendLine($"{methodIndent}    return $\"{className} Instance (No properties)\";");
                }

                sb.AppendLine($"{methodIndent}}}");
                sb.AppendLine($"{classIndent}}}");

                if (!string.IsNullOrEmpty(namespaceName))
                {
                    sb.AppendLine("}");
                }

                // ======== 使用优化编码方式 ========
                var sourceText = SourceText.From(sb.ToString(), Encoding.UTF8);
                spc.AddSource($"{className}_AutoToString.g.cs", sourceText);
            });



            context.RegisterPostInitializationOutput(ctx =>
            {

                const string sourceText = """
namespace SourceGeneratorInCSharp
{
    /// <summary>
    /// 作者:   Harling
    /// 时间:   2025/3/5 13:59:26
    /// 备注:   此文件通过PIToolKit模板创建
    /// </summary>
    public static class HelloWorld
    {
        public static string SayHello()
        {
            return "Hello From Generator";
        }
    }
}
""";
                ctx.AddSource("ExampleGenerator.g.cs", SourceText.From(sourceText, Encoding.UTF8));
            });
        }
    }
}