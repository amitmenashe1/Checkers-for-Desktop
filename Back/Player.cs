using System;

namespace Back
{
    public class Player
    {
        private readonly String name;
        private eXorO XorOe;

        public Player(String i_Name, eXorO i_XorO)
        {
            name = i_Name;
            XorOe = i_XorO;
        }

        public eXorO XorO
        {
            get
            {
                return XorOe;
            }
            set
            {
                XorOe = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}

