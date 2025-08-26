using System;
using System.Collections.Generic;

public static class ServiceLocator
{
private static readonly Dictionary<Type, object> _services = new();


public static void Register<T>(T instance) where T : class
{
_services[typeof(T)] = instance;
}


public static T Resolve<T>() where T : class
{
return _services[typeof(T)] as T;
}


public static bool TryResolve<T>(out T service) where T : class
{
if (_services.TryGetValue(typeof(T), out var obj))
{
service = obj as T; return true;
}
service = null; return false;
}
}
