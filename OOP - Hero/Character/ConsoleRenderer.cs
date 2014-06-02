namespace Character
{
    using System;
    using System.Text;

    public class ConsoleRenderer : IRenderer
    {
        private int renderContextMatrixRows;
        private int renderContextMatrixCols;
        private char[,] renderContextMatrix;

        public int RenderContextMatrixRows
        {
            get
            {
                return this.renderContextMatrixRows;
            }
            private set
            {
                this.renderContextMatrixRows = value;
            }
        }

        public int RenderContextMatrixCols
        {
            get
            {
                return this.renderContextMatrixCols;
            }
            private set
            {
                this.renderContextMatrixCols = value;
            }
        }

        public ConsoleRenderer(int visibleConsoleRows, int visibleConsoleCols)
        {
            renderContextMatrix = new char[visibleConsoleRows, visibleConsoleCols];

            this.RenderContextMatrixRows = renderContextMatrix.GetLength(0);
            this.RenderContextMatrixCols = renderContextMatrix.GetLength(1);

            this.ClearQueue();
        }

        public void EnqueueForRendering(IRenderable obj)
        {
            char[,] objImage = obj.GetImage();

            int imageRows = objImage.GetLength(0);
            int imageCols = objImage.GetLength(1);

            MatrixCoords objTopLeft = obj.GetTopLeft();

            int lastRow = Math.Min(objTopLeft.Row + imageRows, this.renderContextMatrixRows);
            int lastCol = Math.Min(objTopLeft.Col + imageCols, this.renderContextMatrixCols);

            for (int row = obj.GetTopLeft().Row; row < lastRow; row++)
            {
                for (int col = obj.GetTopLeft().Col; col < lastCol; col++)
                {
                    if (row >= 0 && row < renderContextMatrixRows &&
                        col >= 0 && col < renderContextMatrixCols)
                    {
                        if ((objImage[row - obj.GetTopLeft().Row, col - obj.GetTopLeft().Col]) != ' ')
                        {
                            renderContextMatrix[row, col] = objImage[row - obj.GetTopLeft().Row, col - obj.GetTopLeft().Col];
                        }
                    }
                }
            }
        }

        public void RenderAll()
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            
            Console.SetCursorPosition(0, 0);

            StringBuilder scene = new StringBuilder();

            for (int row = 0; row < this.renderContextMatrixRows; row++)
            {
                for (int col = 0; col < this.renderContextMatrixCols; col++)
                {
                    scene.Append(this.renderContextMatrix[row, col]);
                }

                scene.Append(Environment.NewLine);
            }

            Console.WriteLine(scene.ToString());
            //Console.ForegroundColor = ConsoleColor.White;
        }

        public void ClearQueue()
        {
            for (int row = 0; row < this.renderContextMatrixRows; row++)
            {
                for (int col = 0; col < this.renderContextMatrixCols; col++)
                {
                    this.renderContextMatrix[row, col] = ' ';
                }
            }
        }

    }
}
