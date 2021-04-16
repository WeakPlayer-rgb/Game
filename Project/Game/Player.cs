using System;

namespace Game
{
    public class Player : IGameObject
    {
        public Vector Speed { get; set; }
        public double Health { get; set; }
        
        public string GetImage(string path) => "car.png";

        public int DrawPriority(int priority) => 0;
        public void ChangeDirection(KeyButton ctrl)
        {
            
        }
    }
}