using System;

namespace Back
{
    public class GameManager
    {
        public static bool ChangeTurn(ref int io_Turn)
        {
            io_Turn++;
            return (io_Turn % 2 == 0);
        }

        public static bool IsLegalName(String i_Name)
        {
            bool isName = true;
            int i = 0;

            while (isName && i < i_Name.Length)
            {
                isName = (i_Name[i] >= 65 && i_Name[i] <= 90) ||
                    (i_Name[i] >= 97 && i_Name[i] <= 122);
                i++;
            }

            return (i_Name.Length <= 10) && isName && i_Name != "";
        }

        private static bool isValidToMove(Position i_Start, Position i_Destination, Board i_Board, Player i_Player)
        {
            Position[] movePositions = new Position[4];
            bool isMyChecker = false, isAbleToMove = false, isValidToMove = false;

            isMyChecker = (i_Player.XorO.Equals(i_Start.XorO));
            movePositions = i_Board.GetMovePositions(i_Start);
            isAbleToMove = (movePositions[0] != null || movePositions[1] != null || movePositions[2] != null || movePositions[3] != null);
            foreach (Position position in movePositions)
            {
                if (i_Board.IsPositionsAreEquals(i_Destination, position))
                {
                    isValidToMove = true;
                }
            }

            return isMyChecker && isAbleToMove && isValidToMove;
        }

        private static bool isValidToEat(Position i_Start, Position i_Destination, Board i_Board, Player i_Player)
        {
            Position[] eatPositions = new Position[4];
            bool isMyChecker = false, isAbleToEat = false, isValidToEat = false;

            isMyChecker = (i_Player.XorO.Equals(i_Start.XorO));
            eatPositions = i_Board.GetEatPositions(i_Start);
            isAbleToEat = eatPositions[0] != null || eatPositions[1] != null || eatPositions[2] != null || eatPositions[3] != null;
            foreach (Position position in eatPositions)
            {
                if (i_Board.IsPositionsAreEquals(i_Destination, position))
                {
                    isValidToEat= true;
                }
            }

            return isMyChecker && isAbleToEat && isValidToEat;
        }

        public static bool PlayWitnYourChecker(Position i_From, int i_Turn, Board i_Board)
        {
            return (i_Turn % 2 == 1 && i_Board.BoardArr[i_From.Row, i_From.Col].XorO.Equals(eXorO.X) ||
                (i_Turn % 2 == 0 && i_Board.BoardArr[i_From.Row, i_From.Col].XorO.Equals(eXorO.O)));
        }

        public static bool IfStuck(Player i_Player, Board i_Board)
        {
            bool isStuck = true;
            Position[] eatPosArr = new Position[4];
            Position[] movePosArr = new Position[4];

            for (int i = 0; i < i_Board.Size; i++)
            {
                for(int j = 0; j < i_Board.Size ; j++)
                {
                    if (i_Player.XorO.Equals(i_Board.BoardArr[i,j].XorO))
                    {
                        eatPosArr = i_Board.GetEatPositions(i_Board.BoardArr[i, j]);
                        movePosArr = i_Board.GetMovePositions(i_Board.BoardArr[i, j]);
                        if (eatPosArr[0] != null || eatPosArr[1] != null || eatPosArr[2] != null || eatPosArr[3] != null
                            || movePosArr[0] != null || movePosArr[1] != null || movePosArr[2] != null || movePosArr[3] != null)
                        {
                            isStuck = false;
                        }
                    }
                }
            }

            return isStuck;
        }

        public static int MakeStep(Position i_From, Position i_To , Board i_Board, Player i_Player)
        {
            int nextStepStatus = 0;

            Position sourcePosition = i_Board.BoardArr[i_From.Row, i_From.Col];
            Position destinationPosition = i_Board.BoardArr[i_To.Row, i_To.Col];

            if (isValidToEat(sourcePosition, destinationPosition, i_Board, i_Player))
            {
                i_Board.Eat(sourcePosition, destinationPosition, i_Board);
                nextStepStatus = 2;
            }
            else if (isValidToMove(sourcePosition, destinationPosition, i_Board, i_Player))
            {
                i_Board.Move(sourcePosition, destinationPosition, i_Board);
                nextStepStatus = 1;
            }

            return nextStepStatus;
        }

        public static Position[] ComputerMove(Board i_Board)
        {
            Position[] pcMove = new Position[2];
            Position[] optionalCheckersToUse = new Position[i_Board.SizeOfCheckersArray(i_Board.Size)];
            Position[] eatArr = null, moveArr = null, validEats = null, validMoves = null;
            Position checkerToUseSrc = null, destPos = null;
            string str = "";
            int randomCheckerIndexFromEats, randomCheckerIndexFromMoves, k = 0 , randomIdx;
            
            Random random = new Random();

            for (int i = 0 ; i < i_Board.Size ; i++)
            {
                for (int j = 0 ; j < i_Board.Size ; j++)
                {
                    eatArr = i_Board.GetEatPositions(i_Board.BoardArr[i, j]);
                    if (i_Board.BoardArr[i,j].Occupied && i_Board.BoardArr[i, j].XorO.Equals(eXorO.O) &&
                        (eatArr[0] != null || eatArr[1] != null || eatArr[2] != null || eatArr[3] != null))
                    {
                        optionalCheckersToUse[k] = i_Board.BoardArr[i,j];
                        str = "eat";
                        k++;
                    }
                }
            }
            // no eating options, checking for moving options
            if (k == 0)
            {
                for (int i = 0 ; i < i_Board.Size ; i++)
                {
                    for (int j = 0 ; j < i_Board.Size ; j++)
                    {
                        moveArr = i_Board.GetMovePositions(i_Board.BoardArr[i, j]);
                        if (i_Board.BoardArr[i, j].Occupied && i_Board.BoardArr[i, j].XorO.Equals(eXorO.O) &&
                            (moveArr[0] != null || moveArr[1] != null || moveArr[2] != null || moveArr[3] != null))
                        {
                            optionalCheckersToUse[k] = i_Board.BoardArr[i, j];
                            str = "move";
                            k++;
                        }
                    }
                }
            }
            randomIdx = random.Next(0, k);
            checkerToUseSrc = optionalCheckersToUse[randomIdx];
            if (str == "eat")
            {
                validEats = i_Board.GetEatPositions(checkerToUseSrc);
                randomCheckerIndexFromEats = random.Next(0, validEats.Length);
                while (validEats[randomCheckerIndexFromEats] == null)
                {
                    randomCheckerIndexFromEats = random.Next(0, validEats.Length);
                }

                destPos = validEats[randomCheckerIndexFromEats];
                pcMove[0] = new Position(checkerToUseSrc.Row, checkerToUseSrc.Col);
                pcMove[1] = new Position(destPos.Row, destPos.Col);
            }
            if (str == "move")
            {
                validMoves = i_Board.GetMovePositions(checkerToUseSrc);
                randomCheckerIndexFromMoves = random.Next(0, validMoves.Length);
                while (validMoves[randomCheckerIndexFromMoves] == null)
                {
                    randomCheckerIndexFromMoves = random.Next(0, validMoves.Length);
                }

                destPos = validMoves[randomCheckerIndexFromMoves];
                pcMove[0] = new Position(checkerToUseSrc.Row, checkerToUseSrc.Col);
                pcMove[1] = new Position(destPos.Row, destPos.Col);
            }

            return pcMove;
        }
    }
}