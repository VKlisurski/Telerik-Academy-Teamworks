namespace Character
{
    using System;

    public abstract class Humanoid : Character
    {
        private string Task { get; set; }
        private Items armor;
        private int experience = 0;
        private int nextLevel = 3;
        private int dmgBonus = 0;


        public Humanoid(string name, string task, MatrixCoords topLeft, char[,] body)
            : base(name, topLeft, body)
        {
            this.Task = task;
            this.armor = Items.Armor;
        }

        public int DmgBonus
        {
            get
            {
                return this.dmgBonus;
            }
        }

        public override void Update()
        {
        }

        public override void GainExpirience()
        {
            this.experience++;

            if (experience > nextLevel)
            {
                nextLevel *= 2;
            }

            LevelUp();
        }

        public override void LevelUp()
        {
            this.dmgBonus = 1;
        }
    }
}
