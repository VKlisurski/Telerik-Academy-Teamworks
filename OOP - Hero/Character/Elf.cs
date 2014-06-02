namespace Character
{
    public class Elf : Humanoid
    {
        private const int health = 100;

        public Elf(string name, string task, MatrixCoords topLeft)
            : base(name, task, topLeft, new char[,] { { ' ', 'O', ' ' }, { '(', '-', '}' }, { '/', ' ', '\\' } })
        {
            this.Weapon = Items.BowOfJustice;
            base.Health = health;
        }

        public Items Weapon { get; private set; }
    }
}
