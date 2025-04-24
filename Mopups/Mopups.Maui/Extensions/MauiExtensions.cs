namespace Mopups.Extensions;

public static class MauiExtensions
{
    public static IMauiContext FindMauiContext(this Element element, bool fallbackToAppMauiContext = true)
    {
        if (element is IElement { Handler.MauiContext: not null } fe)
            return fe.Handler.MauiContext;

        foreach (var parent in element.GetParentsPath())
        {
            if (parent is IElement { Handler.MauiContext: not null } parentView)
                return parentView.Handler.MauiContext;
        }

        return fallbackToAppMauiContext ? Application.Current?.FindMauiContext() : default;
    }

    internal static IEnumerable<Element> GetParentsPath(this Element self)
    {
        Element current = self;

        while (!IsApplicationOrNull(current.RealParent))
        {
            current = current.RealParent;
            yield return current;
        }
    }

    internal static bool IsApplicationOrNull(object? element) =>
        element is null or IApplication;
}