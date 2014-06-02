namespace Character
{
    public class Bat : Creature
    {
        private const int DefaultHealth = 2;

        public Bat(string name, MatrixCoords topLeft)
            : base("Generic bat", topLeft)
        {
            base.Health = DefaultHealth;
            char[] hlthchr = base.Health.ToString().ToCharArray();
            this.body = new char[,] { { '[', hlthchr[0], ']', ' ', ' ' }, { '/', '^', 'v', '^', (char)92 } };
        }
    }
}