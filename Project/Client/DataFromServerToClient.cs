using System.Collections.Generic;
using System.Drawing;
using NewGame;

namespace Server
{
    public class FirstConnection : DataFromServerToClient
    {
        public Player Player;
        public Dictionary<Point, IGameObject> Map;
    }

    public class DataFromServerToClient
    {
        public List<Bullet> Bullets;
        public List<Player> OtherPlayers;
    }

    public class DataFromClientToServer
    {
        public Point NewPlayerPosition;
        public List<Bullet> NewBullets;
    }
}