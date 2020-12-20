namespace Brickwork.Models
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using Brickwork.Utilities;
    using Brickwork.IO.Contracts;
    using Brickwork.Models.Contracts;

    //Implementation of IArea interface
    public class Area : IArea
    {
        //Define private fields to work with
        private int _row;
        private int _col;
        private int[,] _firstLayer;
        private int[,] _secondLayer;

        private readonly IReader _reader;


        public Area(IReader reader)
        {
            this._reader = reader;
        }

        //Constraints of area size in properties of the class
        public int Row
        {
            get => this._row;
            private set
            {
                if (value % 2 != 0 || (value <= 0 || value >= 100))
                {
                    throw new ArgumentException(ExceptionMessages.INVALID_AREA_ROWS_MESSAGE);
                }

                this._row = value;
            }
        }
        public int Col
        {
            get => this._col;
            private set
            {
                if (value % 2 != 0 || (value <= 0 || value >= 100))
                {
                    throw new ArgumentException(ExceptionMessages.INVALID_AREA_COLUMNS_MESSAGE);
                }

                this._col = value;
            }
        }

        //Method for set size of the area
        public void InitializeArea(int[] input)
        {
            this.Row = input[0];
            this.Col = input[1];

            this._secondLayer = new int[this.Row, this.Col];
            this._firstLayer = new int[this.Row, this.Col];
        }

        //Method for add values into area
        public void AddLines()
        {
            for (int i = 0; i < this._row; i++)
            {
                var line = this._reader.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                this.ValidateLine(line);

                for (int j = 0; j < this._col; j++)
                {
                    this._secondLayer[i, j] = line[j];
                    this._firstLayer[i, j] = line[j];
                }
            }

            this.ValidateBrick();
        }

        //Method for creating a the second layer
        public void CreateSecondLayer()
        {
            //Initialize variables 
            var temp = 0;
            var counter = 0;
            var isNeedToShuffle = this.IsNeedToShuffle();
            var row = this._secondLayer.GetLength(0);
            var col = this._secondLayer.GetLength(1);

            //Condition for different cases
            if (col == 2)
            {
                temp = this._secondLayer[1, 0];
                this._secondLayer[1, 0] = this._secondLayer[0, 1];
                this._secondLayer[0, 1] = temp;
            }
            else
            {
                //While bricks are not constructed correctly shuffle
                while (isNeedToShuffle)
                {
                    for (int i = 0; i < row - 1; i++)
                    {
                        for (int j = 0; j < col - 1; j++)
                        {
                            if (this._secondLayer[i, j] == this._secondLayer[i, j + 1] && this._secondLayer[i + 1, j] == this._secondLayer[i + 1, j + 1] ||
                                this._secondLayer[i, j + 1] == this._secondLayer[i + 1, j + 1] && this._secondLayer[i, j] == this._secondLayer[i + 1, j])
                            {
                                temp = this._secondLayer[i + 1, j];
                                this._secondLayer[i + 1, j] = this._secondLayer[i, j + 1];
                                this._secondLayer[i, j + 1] = temp;
                            }
                            else if (this._secondLayer[i, j] != this._secondLayer[i, j + 1] &&
                                this._secondLayer[i, j + 1] == this._secondLayer[i + 1, j + 1])
                            {

                                this._secondLayer[i + 1, j] = this._secondLayer[i, j + 1];
                                this._secondLayer[i, j + 1] = this._secondLayer[i, j];
                            }
                            else
                            {
                                this._secondLayer[i, j + 1] = this._secondLayer[i, j];
                                this._secondLayer[i, j + 3] = this._secondLayer[i, j + 2];

                                this._secondLayer[i + 1, j] = this._secondLayer[i + 1, j + 1];
                                this._secondLayer[i + 1, j + 2] = this._secondLayer[i + 1, j + 3];

                                j += 2;
                            }

                            j++;
                        }

                        i++;
                    }

                    isNeedToShuffle = this.IsNeedToShuffle();
                    counter++;

                    //If shuffle 10 times and there is no right way to arrange stop the program
                    if (counter == 10)
                    {
                        Environment.Exit(-1);
                    }
                }
            }
        }

        //Method for print second layer
        public string Print()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _secondLayer.GetLength(0); i++)
            {
                for (int j = 0; j < _secondLayer.GetLength(1); j++)
                {
                    sb.Append($"{_secondLayer[i, j]} ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        //Determine if the second layer need shuffle
        private bool IsNeedToShuffle()
        {
            var isTheSameByRow = false;
            var isTheSameByCol = false;
            var row = this._secondLayer.GetLength(0) - 1;
            var counter = 0;

            for (int j = 0; j < this._secondLayer.GetLength(1) - 1; j++)
            {
                if (this._secondLayer[0, j] == this._secondLayer[0, j + 1] &&
                    this._firstLayer[row, j] == this._firstLayer[row, j + 1])
                {
                    isTheSameByRow = true;
                    counter++;
                    break;
                }
            }

            for (int j = 0; j < this._secondLayer.GetLength(1); j++)
            {
                if (this._secondLayer[0, j] == this._secondLayer[1, j] &&
                    this._firstLayer[row, j] == this._firstLayer[row - 1, j])
                {
                    isTheSameByCol = true;
                    counter++;
                    break;
                }
            }

            return isTheSameByRow || isTheSameByCol;
        }

        //Determine if the input of line is correctly inserted
        //If the input is with different size from the column size throw exception
        private void ValidateLine(int[] line)
        {
            if (line.Length != this._secondLayer.GetLength(1))
            {
                throw new Exception(ExceptionMessages.INVALID_SIZE_OF_LINE);
            }
        }

        //Combines all brick validations
        private void ValidateBrick()
        {
            var brickValueDictionary = new Dictionary<int, int>();
            var correctBrickDictionary = new Dictionary<int, bool>();

            this.CorrectValueBrickValidation(brickValueDictionary, correctBrickDictionary);
            this.CorrectBrickValidation(correctBrickDictionary);
            this.CorrectBrickLastRowValidation(correctBrickDictionary);
            this.CorrectBrickLastColumnValidation(correctBrickDictionary);
            this.IsAllAreTrue(correctBrickDictionary);
        }

        //Determine if the value of the brick is rigth and spanning
        private void CorrectValueBrickValidation(Dictionary<int, int> brickValueDictionary, Dictionary<int, bool> correctBrickDictionary)
        {
            for (int i = 0; i < this._secondLayer.GetLength(0); i++)
            {
                for (int j = 0; j < this._secondLayer.GetLength(1); j++)
                {
                    var matrixCurrentPosition = this._secondLayer[i, j];
                    var maxBrickValue = (this._secondLayer.GetLength(0) * this._secondLayer.GetLength(1)) / 2;

                    if (matrixCurrentPosition > maxBrickValue)
                    {
                        throw new Exception($"The value of the brick cannot be more than {maxBrickValue}");
                    }

                    if (!brickValueDictionary.ContainsKey(matrixCurrentPosition))
                    {
                        brickValueDictionary[matrixCurrentPosition] = 0;
                        correctBrickDictionary[matrixCurrentPosition] = false;
                    }

                    brickValueDictionary[matrixCurrentPosition]++;

                    if (brickValueDictionary[matrixCurrentPosition] > 2)
                    {
                        throw new Exception(ExceptionMessages.INVALID_BRICK);
                    }
                }
            }
        }

        //Check for wrong build bricks if there is just one wrong throw exception
        private void IsAllAreTrue(Dictionary<int, bool> correctBrickDictionary)
        {
            foreach (var element in correctBrickDictionary)
            {
                if (element.Value == false)
                {
                    throw new Exception(ExceptionMessages.INVALID_CONSTRUCTED_BRICK);
                }
            }
        }

        //Determine if brick is build correct or not one by one 
        private void CorrectBrickValidation(Dictionary<int, bool> correctBrickDictionary)
        {
            for (int i = 0; i < this._secondLayer.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < this._secondLayer.GetLength(1) - 1; j++)
                {
                    if ((this._secondLayer[i, j] == this._secondLayer[i, j + 1]) || (this._secondLayer[i, j] == this._secondLayer[i + 1, j]))
                    {
                        correctBrickDictionary[this._secondLayer[i, j]] = true;
                    }
                }
            }
        }

        //Check last column bricks
        private void CorrectBrickLastColumnValidation(Dictionary<int, bool> correctBrickDictionary)
        {
            for (int i = 0; i < this._secondLayer.GetLength(0) - 1; i++)
            {
                var a = this._secondLayer.GetLength(1) - 1;

                if (_secondLayer[i, a] == this._secondLayer[i + 1, a])
                {
                    correctBrickDictionary[this._secondLayer[i, a]] = true;
                }
            }
        }

        //Check last row bricks
        private void CorrectBrickLastRowValidation(Dictionary<int, bool> correctBrickDictionary)
        {
            for (int i = 0; i < this._secondLayer.GetLength(1) - 1; i++)
            {
                var a = this._secondLayer.GetLength(0) - 1;

                if (_secondLayer[a, i] == this._secondLayer[a, i + 1])
                {
                    correctBrickDictionary[this._secondLayer[a, i]] = true;
                }
            }
        }
    }
}
