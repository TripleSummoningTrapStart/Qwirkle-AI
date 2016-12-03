using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qwirkle
{
    public delegate bool MakePlayDelegate(List<Tuple<Block, int, int>> play);
    public delegate void UpdateDelegate(Block[] playerHand, int PlayerScore, int ComputerScore, Block[,] Board);
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartForm s = new StartForm();
            Application.Run(s);
            List<UpdateDelegate> l = new List<UpdateDelegate>();
            PrologCommunicator prolog = new PrologCommunicator();
            Controller c = new Controller(l, prolog, s.Difficulty);
            Qwirkle form = new Qwirkle(c.MakePlay, c);
            l.Add(form.UpdateForm);
            Application.Run(form);
        }
}
}