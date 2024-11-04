using MemoryGame;
using System;
using System.Windows.Forms;

// Starta applikationen och visa Form1 (spelfönster)
namespace MemoryGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}