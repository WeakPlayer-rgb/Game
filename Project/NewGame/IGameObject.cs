using System.Drawing;

namespace NewGame
{
    public interface IGameObject
    {
        
        Rectangle ObjRectangle { get; }
        public int MaxHealth();
        double Direction { get;  }
        double Health { get; set; }
        string GetImage();
        int DrawPriority(int priority);
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}