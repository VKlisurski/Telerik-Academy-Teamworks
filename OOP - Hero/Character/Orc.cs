namespace Character
{
    public class Orc : Creature
    {
        private const int DefaultHealth = 5;

        public Orc(string name, MatrixCoords topLeft)
            : base("Generic orc", topLeft)
        {
            base.Health = DefaultHealth;
            char[] hlthchr = base.Health.ToString().ToCharArray();
            this.body = new char[,] { { '[', hlthchr[0], ']' }, { ' ', 'o', ' ' }, { '(', '|', ')' }, { '[', ' ', ']' } };
            
        }
    }
}