namespace Character
{
    public abstract class Character : IRenderable
    {
        protected char[,] body;
        protected MatrixCoords topleft;
        private bool exist;
        
        protected Character(string name, MatrixCoords topLeft, char[,] body)
        {
            this.Name = name;
            this.TopLeft = topLeft;

            int imageRows = body.GetLength(0);
            int imageCols = body.GetLength(1);

            this.body = this.CopyBodyMatrix(body);
            this.exist = true;
        }

        private char[,] CopyBodyMatrix(char[,] matrixToCopy)
        {
            int rows = matrixToCopy.GetLength(0);
            int cols = matrixToCopy.GetLength(1);

            char[,] result = new char[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    result[row, col] = matrixToCopy[row, col];
                }
            }

            return result;
        }

        public bool Exist 
        {
            get
            {
                return this.exist;
            }
        }
        
        public MatrixCoords TopLeft
        {
            get
            {
                return this.topleft;
            }

            set
            {
                this.topleft = value;
            }
        }

        public abstract void Update();

        public virtual void Die()
        {
            this.exist = false;
        }

        public virtual MatrixCoords GetTopLeft()
        {
            return this.TopLeft;
        }

        public char[,] GetImage()
        {
            return this.CopyBodyMatrix(this.body);
        }

        public virtual void MoveLeft()
        {
            this.TopLeft.Col--;
        }

        public virtual void MoveRight()
        {
            this.TopLeft.Col++;
        }

        public virtual void MoveUp()
        {
            this.TopLeft.Row--;
        }

        public virtual void MoveDown()
        {
            this.TopLeft.Row++;
        }

        public MatrixCoords[,] Profile()
        {
            int curRows = (int)this.body.GetLongLength(0);
            int curCols = (int)this.body.GetLongLength(1);
            MatrixCoords[,] profileMatrix = new MatrixCoords[curRows, curCols];
            for (int rows = 0; rows < curRows; rows++)
            {
                for (int cols = 0; cols < curCols; cols++)
                {
                    profileMatrix[rows, cols] = new MatrixCoords(this.topleft.Row + rows, this.topleft.Col + cols);
                }
            }
            return profileMatrix;
        }

        protected int Expirience { get; private set; }

        protected int Level { get; set; }

        protected string Name { get; private set; }

        public int Health { get; set; }

        public abstract void GainExpirience();

        public abstract void LevelUp();
    }
}
