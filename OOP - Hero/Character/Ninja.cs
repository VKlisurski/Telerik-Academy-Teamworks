namespace Character
{
    public class Ninja : Creature
    {
        private const int DefaultHealth = 4;

        public Ninja(string name, MatrixCoords topLeft)
            : base("Generic ninja", topLeft)
        {
            base.Health = DefaultHealth;
            char[] hlthchr = base.Health.ToString().ToCharArray();
            this.body = new char[,] { { '[', hlthchr[0], ']' }, { ' ', 'o', '~' }, { 'v', '|', '>' }, { '<', ' ', (char)92 } };
            
        }
    }
}