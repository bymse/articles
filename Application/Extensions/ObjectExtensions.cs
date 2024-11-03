namespace Application.Extensions;

public static class ObjectExtensions
{
    public static T If<T>(this T obj, bool condition, Func<T, T> action)
    {
        return condition ? action(obj) : obj;
    }
}