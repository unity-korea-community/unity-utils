using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UNKO.Utils;

public class CollectionExtensionTests
{
    [Test]
    public void ToStringCollectionExample()
    {
        string list = new List<int> { 1, 2, 3, 4, 5 }.ToStringCollection();
        Debug.Log(list);

        string dictionary = new Dictionary<string, int>
        {
            {"one", 1}, {"two", 2}, {"three", 3},
        }.ToStringCollection();
        Debug.Log(dictionary);
    }

    [Test]
    public void ForeachExample()
    {
        int[] originArray = new[] { 1, 2, 3, 4, 5 };
        originArray.Foreach(number => Debug.Log(number));
    }

    [Test]
    public void DequeueExample()
    {
        List<int> list = new List<int> { 1, 2, 3 };
        Assert.AreEqual(list.Dequeue(), 1);
        Assert.AreEqual(list.Count, 2);

        while (list.Count > 0)
        {
            list.Dequeue();
        }
        Assert.AreEqual(list.Count, 0);
    }

    [Test]
    public void PopExample()
    {
        List<int> list = new List<int> { 1, 2, 3 };
        Assert.AreEqual(list.Pop(), 3);
        Assert.AreEqual(list.Count, 2);

        while (list.Count > 0)
        {
            list.Dequeue();
        }
        Assert.AreEqual(list.Count, 0);
    }
}
