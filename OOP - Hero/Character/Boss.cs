namespace Character
{
    public class Boss : Creature
    {
        private const int DefaultHealth = 9;

        public Boss(string name, MatrixCoords topLeft)
            : base("Generic orc", topLeft)
        {
            base.Health = DefaultHealth;
            char[] hlthchr = base.Health.ToString().ToCharArray();
            this.body = new char[,] { { '[', hlthchr[0], ']', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }, { ' ', ' ', '_', '_', '_', 'o', ' ', '.', '-', '-', '.', ' ', ' ' }, { ' ', '/', '_', '_', '_', '|', ' ', '|', 'O', 'O', '|', ' ', ' ' }, { '/', (char)39, ' ', ' ', ' ', '|', '_', '|', ' ', ' ', '|', '_', ' ' }, { ' ', ' ', ' ', ' ', ' ', '(', '_', ' ', ' ', ' ', ' ', '_', ')' }, { ' ', ' ', ' ', ' ', ' ', '|', ' ', '|', ' ', ' ', ' ', (char)92, ' ' }, { ' ', ' ', ' ', ' ', ' ', '|', ' ', '|', '_', '_', '_', '/', ' ' } };
            
        }

        public override void CreatureSqueel()
        {
            (new System.Media.SoundPlayer(@"..\..\Beastdeath.wav")).Play();
        }
    }
}