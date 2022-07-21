using System;

namespace Back
{
	public class Board
	{
		private Position[,] board;
		private int xLeft;
		private int oLeft;
		private int xKings;
		private int oKings;
		private int scoreX;
		private int scoreO;
		private readonly int size;
		
		public Board(int i_Size)
		{
			size = i_Size;
			oLeft = SizeOfCheckersArray(i_Size);
			xLeft = SizeOfCheckersArray(i_Size);
			board = new Position[i_Size, i_Size];
			this.board = initBoard(i_Size);
			oKings = 0;
			xKings = 0;
			scoreO = 0;
			scoreX = 0;
		}

		public int XKings
		{
			get
			{
				return xKings;
			}
			set
			{
				xKings = value;
			}
		}

		public int OKings
		{
			get
			{
				return oKings;
			}
			set
			{
				oKings = value;
			}
		}

		public int XLeft
		{
			get
			{
				return xLeft;
			}
			set
			{
				xLeft = value;
			}
		}

		public int OLeft
		{
			get
			{
				return oLeft;
			}
			set
			{
				oLeft = value;
			}
		}

		public int ScoreX
        {
			get
            {
				return scoreX;
            }
			set
            {
				scoreX = value;
            }
        }
		public int ScoreO
		{
			get
			{
				return scoreO;
			}
			set
			{
				scoreO = value;
			}
		}

		public int Size
		{
			get
			{
				return size;
			}
		}

		public Position[,] BoardArr
		{
			get
			{
				return board;
			}
            set
            {
				board = value;

			}
		}

		public void Move(Position i_Start, Position i_Destination, Board i_Board)
		{
			i_Board.BoardArr[i_Destination.Row, i_Destination.Col].Occupied = true;
			i_Board.BoardArr[i_Destination.Row, i_Destination.Col].XorO = i_Start.XorO;
			i_Board.BoardArr[i_Start.Row, i_Start.Col].Occupied = false;

			if (i_Board.BoardArr[i_Start.Row, i_Start.Col].IsKing == true)
            {
				i_Board.BoardArr[i_Start.Row, i_Start.Col].IsKing = false;
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
			}
			if (i_Destination.Row == 0 && i_Destination.XorO.Equals(eXorO.X))
			{
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
				this.XKings++;
			}
			if (i_Destination.Row == size - 1 && i_Destination.XorO.Equals(eXorO.O))
			{
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
				this.OKings++;
			}
		}

		public void Eat(Position i_Start, Position i_Destination, Board i_Board)
		{
			Position posEatenChecker = board[(i_Start.Row + i_Destination.Row) / 2,
				     (i_Start.Col + i_Destination.Col) / 2];

			if (i_Board.BoardArr[i_Start.Row, i_Start.Col].IsKing)
            {
				i_Board.BoardArr[i_Start.Row, i_Start.Col].IsKing = false;
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
			}
			i_Board.BoardArr[i_Destination.Row, i_Destination.Col].Occupied = true;
			i_Board.BoardArr[i_Destination.Row, i_Destination.Col].XorO = i_Start.XorO;
			i_Board.BoardArr[i_Start.Row, i_Start.Col].Occupied = false;
			i_Board.BoardArr[posEatenChecker.Row, posEatenChecker.Col].Occupied = false;

			if (i_Board.BoardArr[i_Destination.Row, i_Destination.Col].XorO.Equals(eXorO.X))
            {
				if (i_Board.BoardArr[posEatenChecker.Row, posEatenChecker.Col].IsKing)
				{
					ScoreX += 4;
				}
				else
                {
					ScoreX++;
				}

				i_Board.BoardArr[posEatenChecker.Row, posEatenChecker.Col].IsKing = false;
				OLeft--;
            }
			else if (i_Board.BoardArr[i_Destination.Row, i_Destination.Col].XorO.Equals(eXorO.O))
			{
				if (BoardArr[posEatenChecker.Row, posEatenChecker.Col].IsKing)
				{
					ScoreO += 4;
				}
				else
				{
					ScoreO++;
				}
				i_Board.BoardArr[posEatenChecker.Row, posEatenChecker.Col].IsKing = false;
				XLeft--;
			}

			if (i_Destination.Row == 0 && i_Destination.XorO.Equals(eXorO.X))
			{
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
				xKings++;
			}
			if (i_Destination.Row == size - 1 && i_Destination.XorO.Equals(eXorO.O))
			{
				i_Board.BoardArr[i_Destination.Row, i_Destination.Col].IsKing = true;
				oKings++;
			}	
		}

		public String Winner()
		{
			String winnerType = null;

			if (xLeft == 0)
			{
				winnerType = "O";
			}
			if (oLeft == 0)
			{
				winnerType = "X";
			}

			return winnerType;
		}

		public int SizeOfCheckersArray(int i_Size)
		{
			int sizeArr = 0;

			if (i_Size == 6)
			{
				sizeArr = 6;
			}
			if (i_Size == 8)
			{
				sizeArr = 12;
			}
			if (i_Size == 10)
			{
				sizeArr = 20;
			}

			return sizeArr;
		}

		private Position[,] initBoard(int i_Size)
		{
			int i = 0, j = 0;
			Position[,] board = new Position[i_Size, i_Size];

			for (i = 0 ; i < i_Size ; i++)
			{
				for (j = 0 ; j < i_Size ; j++)
				{
					board[i, j] = new Position(i, j);
					board[i, j].IsKing = false;
				}
			}
			for (i = i_Size - 1 ; i > i_Size / 2 ; i--)
			{
				for (j = 0 ; j < i_Size ; j++)
				{
					if ((j % 2) == ((i + 1) % 2))
					{
						board[i, j].Occupied = true;
						board[i, j].XorO = eXorO.X;
					}
				}
			}
			for (i = 0 ; i < (i_Size / 2) - 1 ; i++)
			{
				for (j = i_Size - 1 ; j >= 0 ; j--)
				{
					if ((j % 2) == ((i + 1) % 2))
					{
						board[i, j].Occupied = true;
						board[i, j].XorO = eXorO.O;
					}
				}
			}

			return board;
		}

		public bool IsInBoard(Position i_PositionToCheck)
        {
			return (i_PositionToCheck.Row >= 0 && i_PositionToCheck.Row < size && i_PositionToCheck.Col >= 0 && i_PositionToCheck.Col < size);
        }

		public Position[] GetMovePositions(Position i_Start)
		{
			Position[] positionsNotOutOfBound = new Position[4];
			Position upRight = new Position(i_Start.Row - 1, i_Start.Col + 1);
			Position upLeft = new Position(i_Start.Row - 1, i_Start.Col - 1);
			Position downRight = new Position(i_Start.Row + 1, i_Start.Col + 1);
			Position downLeft = new Position(i_Start.Row + 1, i_Start.Col - 1);
			Position[] positionsToCheckValid = {upRight, upLeft, downRight, downLeft};
			int i = 0;

			// king X
			if (i_Start.IsKing && i_Start.XorO.Equals(eXorO.X))
			{
				foreach (Position position in positionsToCheckValid)
				{
					if (IsInBoard(position))
                    {
						if (BoardArr[position.Row, position.Col].Occupied == false)
                        {
							positionsNotOutOfBound[i] = position;
							i++;
						}
                    }
				}
			}

			// regular X
			else if (i_Start.XorO.Equals(eXorO.X) && i_Start.IsKing == false)
			{
				if (IsInBoard(upRight) && BoardArr[upRight.Row, upRight.Col].Occupied == false)
				{
					positionsNotOutOfBound[0] = upRight;
				}
				if (IsInBoard(upLeft) && BoardArr[upLeft.Row, upLeft.Col].Occupied == false)
				{
					positionsNotOutOfBound[1] = upLeft;
				}
			}

			// king O
			else if (i_Start.XorO.Equals(eXorO.O) && i_Start.IsKing)
			{
				foreach (Position position in positionsToCheckValid)
				{
					if (IsInBoard(position))
					{
						if (BoardArr[position.Row, position.Col].Occupied == false)
						{
							positionsNotOutOfBound[i] = position;
							i++;
						}
					}
				}
			}

			// regular O
			else if (i_Start.XorO.Equals(eXorO.O) && i_Start.IsKing == false)
			{
				if (IsInBoard(downLeft) && BoardArr[downLeft.Row, downLeft.Col].Occupied == false)
				{
					positionsNotOutOfBound[0] = downLeft;
				}
				if (IsInBoard(downRight) && BoardArr[downRight.Row, downRight.Col].Occupied == false)
				{
					positionsNotOutOfBound[1] = downRight;
				}
			}

			return positionsNotOutOfBound;
		}

		public Position[] GetEatPositions(Position i_Start)
		{
			int i = 0, k = 0, t = 0;
			Position[] EatPositions = new Position[4];
			Position[] positionToCheckEnemyChecker = new Position[4];
			Position[] positionsToCheckEmpty = new Position[4];
			Position upRight = new Position(i_Start.Row - 1, i_Start.Col + 1);
			Position upLeft = new Position(i_Start.Row - 1, i_Start.Col - 1);
			Position downRight = new Position(i_Start.Row + 1, i_Start.Col + 1);
			Position downLeft = new Position(i_Start.Row + 1, i_Start.Col - 1);
			Position upUpRight = new Position(i_Start.Row - 2, i_Start.Col + 2);
			Position upUpLeft = new Position(i_Start.Row - 2, i_Start.Col - 2);
			Position downDownRight = new Position(i_Start.Row + 2, i_Start.Col + 2);
			Position downDownLeft = new Position(i_Start.Row + 2, i_Start.Col - 2);
			Position[] directions2 = { upUpRight, upUpLeft, downDownRight, downDownLeft };
			Position[] directions1 = { upRight, upLeft, downRight, downLeft };

			// adds to the arrays to check only the directions that are in board
			if (BoardArr[i_Start.Row, i_Start.Col].IsKing)
			{
				for (int j = 0 ; j < 4 ; j++)
				{
					if (IsInBoard(directions1[j]) && IsInBoard(directions2[j]))
					{
						positionToCheckEnemyChecker[i] = BoardArr[directions1[j].Row, directions1[j].Col];
						positionsToCheckEmpty[i] = BoardArr[directions2[j].Row, directions2[j].Col];
						i++;
					}
				}
			}
			// king X - need to check 4 directions
			if (BoardArr[i_Start.Row, i_Start.Col].IsKing && BoardArr[i_Start.Row, i_Start.Col].XorO.Equals(eXorO.X))
			{
				for (int l = 0 ; l < positionsToCheckEmpty.Length ; l++)
				{
					if (positionToCheckEnemyChecker[l] == null || positionsToCheckEmpty[l] == null)
					{
						l++;
					}
					else if (positionToCheckEnemyChecker[l].Occupied && positionToCheckEnemyChecker[l].XorO.Equals(eXorO.O)
						&& positionsToCheckEmpty[l].Occupied == false)
					{
						EatPositions[t] = positionsToCheckEmpty[l];
						t++;
					}
				}
			}

			// king O - needs to check all 4 directions
			else if (BoardArr[i_Start.Row, i_Start.Col].IsKing && BoardArr[i_Start.Row, i_Start.Col].XorO.Equals(eXorO.O))
			{

				for (int l = 0 ; l < positionsToCheckEmpty.Length ; l++)
				{
					if (positionToCheckEnemyChecker[l] == null || positionsToCheckEmpty[l] == null)
                    {
						l++;
                    }
					else if (positionToCheckEnemyChecker[l].Occupied && positionToCheckEnemyChecker[l].XorO.Equals(eXorO.X)
						&& positionsToCheckEmpty[l].Occupied == false)
					{
						EatPositions[t] = positionsToCheckEmpty[l];
						t++;
					}
				}
			}
			// regular checker X
			else if (BoardArr[i_Start.Row, i_Start.Col].IsKing == false && BoardArr[i_Start.Row, i_Start.Col].XorO.Equals(eXorO.X))
			{
				if (IsInBoard(upRight) && IsInBoard(upUpRight))
				{
					if (BoardArr[upRight.Row, upRight.Col].Occupied && BoardArr[upRight.Row, upRight.Col].XorO.Equals(eXorO.O)
												&& BoardArr[upUpRight.Row, upUpRight.Col].Occupied == false)
					{
						EatPositions[k] = upUpRight;
						k++;
					}
				}

				if (IsInBoard(upLeft) && IsInBoard(upUpLeft))
				{
					if (BoardArr[upLeft.Row, upLeft.Col].Occupied && BoardArr[upLeft.Row, upLeft.Col].XorO.Equals(eXorO.O)
												&& !BoardArr[upUpLeft.Row, upUpLeft.Col].Occupied)
					{
						EatPositions[k] = upUpLeft;
						k++;
					}
				}
			}
			// regular checker O
			else if (BoardArr[i_Start.Row, i_Start.Col].IsKing == false && BoardArr[i_Start.Row, i_Start.Col].XorO.Equals(eXorO.O))
			{
				if (IsInBoard(downLeft) && IsInBoard(downDownLeft))
				{
					if (BoardArr[downLeft.Row, downLeft.Col].Occupied && BoardArr[downLeft.Row, downLeft.Col].XorO.Equals(eXorO.X)
												&& !BoardArr[downDownLeft.Row, downDownLeft.Col].Occupied)
					{
						EatPositions[k] = downDownLeft;
						k++;
					}
				}
				if (IsInBoard(downRight) && IsInBoard(downDownRight))
				{
					if (BoardArr[downRight.Row, downRight.Col].Occupied && BoardArr[downRight.Row, downRight.Col].XorO.Equals(eXorO.X)
												&& !BoardArr[downDownRight.Row, downDownRight.Col].Occupied)
					{
						EatPositions[k] = downDownRight;
						k++;
					}
				}
			}
		
			return EatPositions;
		}

		public bool IsPositionsAreEquals(Position i_Position1, Position i_Positions2)
		{
			return (i_Positions2 != null) && (i_Position1.Row == i_Positions2.Row) && (i_Position1.Col == i_Positions2.Col);
		}
	}
}