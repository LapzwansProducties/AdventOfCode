using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class PuzzleAttribute : Attribute, ITestBuilder, IImplyFixture
{
    public PuzzleAttribute(object answer)
    {
        Answer = answer;
    }

    public object Answer { get; }

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var location = new DirectoryInfo(".");
        var type = method.MethodInfo.DeclaringType!;
        var year = type.Namespace!.Split('.')[^1][1..];
        var file = new FileInfo(Path.Combine(location.FullName, "..", "..", "..", year, $"{type.Name}.txt"));


        var parameters = new TestCaseParameters([Input(file)])
        {
            ExpectedResult = Answer,
        };
        var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters);
        test.Name =
            file.Exists
            ? $"answer: {Answer}, {method.Name}"
            : "File does not exist";
        return [test];
    }

    private static string? Input(FileInfo file)
    {
        if (!file.Exists) return null;
        using var reader = file.OpenText();
        return reader.ReadToEnd();
    }
}
