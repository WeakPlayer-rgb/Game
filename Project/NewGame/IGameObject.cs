using System.Drawing;

namespace NewGame
{
    public interface IGameObject
    {
        
        Rectangle ObjRectangle { get; }
        /// <summary>
        /// Angle of movement
        /// </summary>
        double Direction { get; }
        double Health { get; set; }
        string GetImage();
        public int MaxHealth();
        int DrawPriority(int priority);
        /// <summary>
        /// Change angle
        /// </summary>
        /// <param name="ctrl">Button tapped</param>
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}