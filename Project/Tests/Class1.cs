using System;
using System.Drawing;
using NUnit.Framework;
using NewGame;

namespace ClassLibrary1
{
    public class Class1
    {
        private GameModel model;

        [SetUp]
        public void SetUp()
        {
            model = new GameModel(10);
        }
        
        [Test]
        public void TurnTest()
        {
            var car = new Player(new Point(0, 0));
            car.ChangeVelocity(KeyButton.Forward);
            car.ChangeDirection(KeyButton.Left);
            Console.WriteLine(car.Direction);
        }

    //     [TestCase(-1, -1, 1, 1, -1, 1, -1, 1)]
    //     [TestCase(-2, -1, 0, 2, -1, 1, -1, 1)]
    //     [TestCase(0, -2, 2, 1, -1, 1, -1, 1)]
    //     [TestCase(0, 2, 2, -1, -1, 1, -1, 1)]
    //     [TestCase(-2,1, 0, -1, -1, 1, -1, 1)]
    //     public void IntersectionTest(params int[] ints)
    //     {
    //         var intersected = model.IsIntersected(new Vector(ints[0], ints[1]),
    //             new Vector(ints[2], ints[3]), ints[4], ints[5], ints[6], ints[7]);
    //         Assert.IsTrue(intersected);
    //     }
    }
}