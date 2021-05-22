// using System.Drawing;
//
// namespace NewGame
// {
//     public interface IGameObject
//     {
//         public abstract Point Position { get; set; }
//         public Rectangle ObjRectangle { get; }
//         /// <summary>
//         /// Angle of movement
//         /// </summary>
//         public abstract double Direction { get; }
//
//         public abstract double Health { get; set; }
//         public abstract string GetImage();
//         public abstract int MaxHealth();
//         public abstract int DrawPriority(int priority);
//
//         /// <summary>
//         /// Change angle
//         /// </summary>
//         /// <param name="ctrl">Button tapped</param>
//         public abstract void ChangeDirection(KeyButton ctrl);
//
//         public abstract void ChangeVelocity(KeyButton ctrl);
//     }
// }