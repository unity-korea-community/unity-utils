using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UNKO;
using UNKO.Utils;

public class SimplePoolTests
{
    public class SimplePoolTarget
    {
        public static int InstanceCount { get; private set; }

        public static void Reset_InstanceCount()
        {
            InstanceCount = 0;
        }

        static public class Factory
        {
            static public SimplePoolTarget CreateInstance_FromFactory()
            {
                return new SimplePoolTarget { IsCreateFromFactory = true };
            }
        }

        public bool IsCreateFromFactory { get; private set; }

        public SimplePoolTarget()
        {
            InstanceCount++;
        }
    }

    [Test]
    public void 생성자에서_미리풀에생성할수있습니다()
    {
        SimplePoolTarget origin = new SimplePoolTarget();
        SimplePoolTarget.Reset_InstanceCount(); // origin을 생성하면서 추가된 instance count 초기화

        int instanceCount = Random.Range(1, 10);
        SimplePool<SimplePoolTarget> pool = new SimplePool<SimplePoolTarget>(origin, instanceCount);

        Assert.AreEqual(pool.AllInstance.Count, instanceCount);
        Assert.AreEqual(SimplePoolTarget.InstanceCount, instanceCount);
    }

    [Test]
    public void 사용예시()
    {
        SimplePoolTarget.Reset_InstanceCount();
        int totalInstanceCount = 10;
        SimplePool<SimplePoolTarget> pool = new SimplePool<SimplePoolTarget>(new SimplePoolTarget(), totalInstanceCount);
        int loopCount = Random.Range(3, 10);
        for (int i = 0; i < loopCount; i++)
        {
            int spawnCount = Random.Range(2, totalInstanceCount);
            HashSet<SimplePoolTarget> set = new HashSet<SimplePoolTarget>();
            for (int j = 0; j < spawnCount; j++)
            {
                set.Add(pool.Spawn());
            }

            foreach (var item in set)
            {
                pool.DeSpawn(item);
            }
        }

        Assert.AreEqual(pool.AllInstance.Count, totalInstanceCount);
        Assert.AreEqual(pool.Use.Count, 0);
        Assert.AreEqual(pool.NotUse.Count, totalInstanceCount);
    }

    public class PoolEx : SimplePool<SimplePoolTarget>
    {
        public PoolEx(int initializeSize) : base(new SimplePoolTarget(), initializeSize)
        {
        }

        protected override SimplePoolTarget OnRequireNewInstance(SimplePoolTarget originItem)
        {
            return SimplePoolTarget.Factory.CreateInstance_FromFactory();
        }
    }

    [Test]
    public void 사용예시_생성자Override()
    {
        SimplePoolTarget.Reset_InstanceCount();
        int instanceCount = Random.Range(1, 10);
        PoolEx poolEx = new PoolEx(instanceCount);
        Assert.AreEqual(poolEx.AllInstance.Count, instanceCount);

        SimplePoolTarget target = poolEx.Spawn();
        Assert.AreEqual(target.IsCreateFromFactory, true);
    }
}
