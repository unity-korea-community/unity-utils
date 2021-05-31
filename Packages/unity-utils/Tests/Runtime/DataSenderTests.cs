using System;
using NUnit.Framework;
using UnityEngine;
using UNKO.Utils;

public class DataSenderTests
{
    public class TestData
    {
        public string stringData;
        public int numberData;

        public TestData(int numberData)
        {
            this.stringData = numberData.ToString();
            this.numberData = numberData;
        }
    }

    public class TestSender : MonoBehaviour
    {
        public DataSender<TestData> sender { get; private set; } = new DataSender<TestData>();
    }

    public class TestReceiver : MonoBehaviour, IObserver<TestData>
    {
        public TestData data { get; private set; }

        public void OnCompleted()
        {
            // Do nothing because test
        }

        public void OnError(Exception error)
        {
            // Do nothing because test
        }

        public void OnNext(TestData value)
        {
            this.data = value;
        }
    }

    [Test]
    public void 사용예시()
    {
        // Arrange
        TestSender senderComponent = new GameObject(nameof(TestSender)).AddComponent<TestSender>();
        TestReceiver receiverComponent = new GameObject(nameof(TestReceiver)).AddComponent<TestReceiver>();
        receiverComponent.transform.SetParent(senderComponent.transform);
        senderComponent.sender.InitChildrenComponents(senderComponent);

        // Act
        TestData testData = new TestData(UnityEngine.Random.Range(1, 100));
        senderComponent.sender.SendData(testData);

        // Assert
        Assert.AreEqual(receiverComponent.data.stringData, testData.stringData);
        Assert.AreEqual(receiverComponent.data.numberData, testData.numberData);
    }
}