using System.IO;
namespace Character
{
    public class Knight : Humanoid
    {
        private Items weapon;
        private Items shield;
        private const int DefaultHealth = 1000;

        public Knight(string name, string task, MatrixCoords topLeft)
            : base(name, task, topLeft, new char[,] { { ' ', 'O', ' ' }, { '|', '-', '>' }, { '/', ' ', '\\' } })
        {
            this.Weapon = Items.Sword;
            this.Shield = Items.Shield;
            base.Health = DefaultHealth;
            this.IsSlashing = false;
        }

        public Items Weapon { get; private set; }
        
        public Items Shield { get; private set; }

        public bool IsSlashing { get; set; }

        public void Slash()
        {
            this.body = new char[,] { { ' ', 'O', ' ', ' ', ' ' }, { '|', '-', '=', '=', '>' }, { '/', ' ', '\\', ' ', ' ' } };
            this.IsSlashing = true;
            
            try
            {
                (new System.Media.SoundPlayer(@"..\..\Kick.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException("File was not found!");
            }
        }

        public void UnSlash()
        {
            this.body = new char[,] { { ' ', 'O', ' ' }, { '|', '-', '>' }, { '/', ' ', '\\' } };
            this.IsSlashing = false;
        }
    }
}
