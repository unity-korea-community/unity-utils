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
        int[] numbers = new[] { 1, 2, 3, 4, 5 };
        Debug.Log(numbers.Random());
        Debug.Log(numbers.Random());
    }

    public class Item
    {
        public string Name { get; private set; }
        public int Percent { get; private set; }

        public Item(string name, int percent)
        {
            this.Name = name;
            this.Percent = percent;
        }
    }

    [Test]
    public void RandomPercent()
    {
        // Arrange
        Item[] items = new[] {
            new Item("normal sword", 80),
            new Item("epic sword", 19),
            new Item("regendary sword", 1),
        };

        int gotchaCount = 1000;
        Dictionary<string, int> hasItemCount = new Dictionary<string, int>
        {
            {"normal", 0}, {"epic", 0}, {"regendary", 0}
        };

        // Act
        for (int i = 0; i < gotchaCount; i++)
        {
            Item gotcha = items.Random((item, index) => item.Percent);
            foreach (var hasItem in hasItemCount)
            {
                string key = hasItem.Key;
                if (gotcha.Name.StartsWith(key))
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
            float errorRate = 0.5f; // 랜덤에 걸리지 않기 위해 범위를 많이 넓힘
            int expectCount = (int)(gotchaCount * (item.Percent / 100f));
            int errorRange = (int)(expectCount * errorRate);
            KeyValuePair<string, int> itemCount = hasItemCount.First(hasItem => item.Name.StartsWith(hasItem.Key));

            Assert.GreaterOrEqual(itemCount.Value, expectCount - errorRange);
        }
    }

    [Test]
    public void RandomFilter()
    {
        int[] numbers = new[] { 1, 2, 3, 4, 5 };
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
