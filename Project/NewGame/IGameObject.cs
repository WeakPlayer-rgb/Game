using System.Drawing;

namespace NewGame
{
    public interface IGameObject
    {
        
        Rectangle ObjRectangle { get; }
        public int MaxHealth();
        /// <summary>
        /// Angle of movement
        /// </summary>
        double Direction { get; }
        double Health { get; set; }
        string GetImage();
        int DrawPriority(int priority);
        /// <summary>
        /// Change angle
        /// </summary>
        /// <param name="ctrl">Button tapped</param>
        void ChangeDirection(KeyButton ctrl);
        void ChangeVelocity(KeyButton ctrl);
    }
}