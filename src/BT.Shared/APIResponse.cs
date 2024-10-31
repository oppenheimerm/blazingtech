
namespace BT.Shared
{
    //  Where the uniary postfix operator "!" (null-forgiving operator)
    //  tells the compiler that passing null is expected and shouldn't be
    //  warned about.
    //  See: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
    public record Response(bool flag = false, string message = null!);
}
