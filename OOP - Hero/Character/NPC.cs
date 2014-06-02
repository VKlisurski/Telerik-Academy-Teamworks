namespace Character
{
    using System;
    using System.Collections.Generic;

    public class NPC : Humanoid, IStatic
    {
        Random randomGenerator = new Random();
        private List<string> tasks = new List<string> {"Kill one creature!", " Kill two creatures!", "Kill all creatures,  but leave the boss!", "Kill the Boss of the creatures!", "Kill every one!" }; // to add tasks

        public NPC(string name, string task, MatrixCoords topLeft)
            : base(name, task, topLeft, new char[,] { { ' ', 'O', ' ' }, { '-', '|', '-' }, { '/', ' ', '\\' } })
        {
        }

        public string GiveTask()
        {
            return randomGenerator.Next(0, 2).ToString();
        }
    }
}