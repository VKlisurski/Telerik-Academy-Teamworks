namespace Character
{
    using System;

    public abstract class Creature : Character
    {
        private const int DefaultDmg = 3;

        private int verticalRange = 12;
        private int horizontalRange = 25;
        public bool isSlashing = false;

        public Creature(string name, MatrixCoords topLeft)
            : base(name, topLeft, new char[,] { { 'O' } })
        {

        }

        public int CreatureDamage
        {
            get
            {
                return DefaultDmg;
            }
        }

        public int HorizontalRange
        {
            get
            {
                return this.horizontalRange;
            }
            set
            {
                this.horizontalRange = value;
            }
        }

        public int VerticalRange
        {
            get
            {
                return this.verticalRange;
            }
            set
            {
                this.verticalRange = value;
            }
        }

        public void UpdateHealth(int newHP)
        {
            char[] hlthchr = newHP.ToString().ToCharArray();
            base.body[0, 1] = hlthchr[0];
        }

        public virtual void CreatureSqueel()
        {
            (new System.Media.SoundPlayer(@"..\..\Pain.wav")).Play();
        }

        public override void Update()
        {
        }

        public override void GainExpirience()
        {
            throw new NotImplementedException();
        }

        public override void LevelUp()
        {
            throw new NotImplementedException();
        }
    }
}
