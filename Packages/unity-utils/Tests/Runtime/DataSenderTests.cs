using System;
using NUnit.Framework;
using UnityEngine;
using UNKO.Utils;

public class DataSenderTests
{
    public class TestData
    {
        public string StringData { get; private set; }
        public int NumberData { get; private set; }

        public TestData(int numberData)
        {
            this.StringData = numberData.ToString();
            this.NumberData = numberData;
        }
    }

    public class TestSender : MonoBehaviour
    {
        public DataSender<TestData> Sender { get; private set; } = new DataSender<TestData>();
    }

    public class TestReceiver : MonoBehaviour, IObserver<TestData>
    {
        public TestData Data { get; private set; }

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
            this.Data = value;
        }
    }

    [Test]
    public void 사용예시()
    {
        // Arrange
        TestSender senderComponent = new GameObject(nameof(TestSender)).AddComponent<TestSender>();
        TestReceiver receiverComponent = new GameObject(nameof(TestReceiver)).AddComponent<TestReceiver>();
        receiverComponent.transform.SetParent(senderComponent.transform);
        senderComponent.Sender.InitChildrenComponents(senderComponent);

        // Act
        TestData testData = new TestData(UnityEngine.Random.Range(1, 100));
        senderComponent.Sender.SendData(testData);

        // Assert
        Assert.AreEqual(receiverComponent.Data.StringData, testData.StringData);
        Assert.AreEqual(receiverComponent.Data.NumberData, testData.NumberData);
    }
}
