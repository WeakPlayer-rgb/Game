using NUnit.Framework;
using Game;

namespace TestProject1
{
    public class Test
    {
        [Test]
        public void TryTurnLeft()
        {
            var player = new Game.Player(new Vector(0, 0));
            player.ChangeDirection(KeyButton.Left);
            Assert.Equals(player.Direction, new Vector(-1, 0));
        }
    }
}