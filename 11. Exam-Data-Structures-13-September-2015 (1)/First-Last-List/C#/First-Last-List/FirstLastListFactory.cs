using System;
using First_Last_List;

public static class FirstLastListFactory
{
    public static IFirstLastList<T> Create<T>()
        where T : IComparable<T>
    {
        return new FirstLastList<T>();
    }
}
