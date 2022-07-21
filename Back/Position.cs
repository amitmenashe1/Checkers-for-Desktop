using System;

namespace Back
{

    public enum eXorO
    {
        X,
        O
    }

    public class Position
    {
        private int col;
        private int row;
        private bool isOccupied;
        private eXorO XorOe;
        private bool isKing;
    
        public Position(int i_Row, int i_Col)
        {
            col = i_Col;
            row = i_Row;
            isOccupied = false;
            isKing = false;
        }

        public int Col 
        {
            get
            {
                return col;
            }
            set
            {
                col = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
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

        public bool Occupied
        {
            get
            {
                return isOccupied;
            }
            set
            {
                isOccupied = value;
            }
        }

        public bool IsKing
        {
            get
            {
                return isKing;
            }
            set
            {
                isKing = value;
            }
        }
    }
}
