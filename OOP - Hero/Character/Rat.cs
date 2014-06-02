namespace Character
{
    public class Rat : Creature
    {
        private const int DefaultHealth = 3;

        public Rat(string name, MatrixCoords topLeft)
            : base("Generic rat", topLeft)
        {
            base.Health = DefaultHealth;
            char[] hlthchr = base.Health.ToString().ToCharArray();
            this.body = new char[,] { { '[', hlthchr[0], ']', ' ', ' ' }, { '(', ')', '-', '(', ')' }, { ' ', (char)92, '"', '/', ' ' }, { ' ', ' ', '`', ' ', ' ' } };
            
        }
    }
}