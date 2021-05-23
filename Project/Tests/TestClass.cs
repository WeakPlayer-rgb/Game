using System;
using System.Drawing;
using NUnit.Framework;
using NewGame;

namespace ClassLibrary1
{
    public class TestClass
    {
        private GameModel model;
        private Player car;

        [SetUp]
        public void SetUp()
        {
            model = new GameModel(10, "1");
            car = new Player(new Point(model.Size/2,model.Size/2 ), 0.12f);
        }
        
        [Test]
        public void TurnTest()
        {
            car.ChangeVelocity(KeyButton.Forward);
            car.ChangeDirection(KeyButton.Left);
            Console.WriteLine(car.Direction);
        }

        [TestCase(-1, -1, 1, 1, -1, 1, -1, 1)]
        [TestCase(-2, -1, 0, 2, -1, 1, -1, 1)]
        [TestCase(0, -2, 2, 1, -1, 1, -1, 1)]
        [TestCase(0, 2, 2, -1, -1, 1, -1, 1)]
        [TestCase(-2,1, 0, -1, -1, 1, -1, 1)]
        public void IntersectionTest(params int[] ints)
        {
            /*var intersected = model.IsIntersected(new Vector(ints[0], ints[1]),
                new Vector(ints[2], ints[3]), ints[4], ints[5], ints[6], ints[7]);
            Assert.IsTrue(intersected);
        */
        }

        [TestCase(1,3,4,100, 1000)]
        [TestCase(1,3,4,100, 50)]
        [TestCase(1, 99)]
        public void DamageMakeDeadTest(params int[] damages)
        {
            foreach (var damage in damages)
            {
                car.Damage += damage;
            }
            Assert.IsTrue(car.Health < 0);
        }
        
        [TestCase(1,32,4,5,6)]
        [TestCase(1,4,5,6)]
        public void DamageStillAliveTest(params int[] damages)
        {
            foreach (var damage in damages)
            {
                car.Damage += damage;
            }
            Assert.IsTrue(car.Health > 0);
        }
        
        
    }
}