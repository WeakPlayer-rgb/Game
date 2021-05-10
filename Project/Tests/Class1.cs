using System;
using System.Drawing;
using NUnit.Framework;
using NewGame;

namespace ClassLibrary1
{
    public class Class1
    {
        [Test]
        public void TurnTest()
        {
            var car = new Player(new Point(0, 0));
            car.ChangeVelocity(KeyButton.Forward);
            car.ChangeDirection(KeyButton.Left);
            Console.WriteLine(car.Direction);
        }
    }
}