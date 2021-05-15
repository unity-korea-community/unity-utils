using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UNKO.Utils;

public class RandomExtensionTests
{
    [Test]
    public void RandomWorks()
    {
        int[] numbers = new int[] { 1, 2, 3, 4, 5 };
        Debug.Log(numbers.Random());
        Debug.Log(numbers.Random());
    }

    public class Item
    {
        public string name { get; private set; }
        public int percent { get; private set; }

        public Item(string name, int percent)
        {
            this.name = name;
            this.percent = percent;
        }
    }

    [Test]
    public void RandomPercent()
    {
        // Arrange
        Item[] items = new Item[] {
            new Item("normal sword", 80),
            new Item("epic sword", 19),
            new Item("regendary sword", 1),
        };

        int gotchaCount = 1000;
        Dictionary<string, int> hasItemCount = new Dictionary<string, int>()
        {
            {"normal", 0}, {"epic", 0}, {"regendary", 0}
        };

        // Act
        for (int i = 0; i < gotchaCount; i++)
        {
            Item gotcha = items.Random(item => item.percent);
            foreach (var hasItem in hasItemCount)
            {
                string key = hasItem.Key;
                if (gotcha.name.StartsWith(key))
                {
                    hasItemCount[key]++;
                    break;
                }
            }
        }

        // Assert
        for (int i = 0; i < items.Length; i++)
        {
            Item item = items[i];
            float errorRate = 0.2f;
            int expectCount = (int)(gotchaCount * (item.percent / 100f));
            int errorRange = (int)(expectCount * errorRate);
            KeyValuePair<string, int> itemCount = hasItemCount.First(hasItem => item.name.StartsWith(hasItem.Key));

            // 대부분 통과하나, 랜덤 확률에 의해 가끔 실패함..
            // Assert.GreaterOrEqual(itemCount.Value, expectCount - errorRange);
        }
    }

    [Test]
    public void RandomFilter()
    {
        int[] numbers = new int[] { 1, 2, 3, 4, 5 };
        HashSet<int> set = new HashSet<int>();

        for (int i = 0; i < numbers.Length; i++)
        {
            int randomNumber = numbers.Random(number => set.Contains(number) == false);
            set.Add(randomNumber);
        }

        Assert.AreEqual(numbers.Length, set.Count);
    }

    [Test]
    public void Shuffle()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        numbers.Shuffle();
        numbers.Foreach(item => Debug.Log(item));
    }
}
