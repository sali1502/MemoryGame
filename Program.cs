/* DT071G Programmering i C#.NET Projektuppgift. Åsa Lindskog sali1502@student.miun.se */
/* En memoryspel med topplista baserat på antal försök att matcha bildpar. */

using MemoryGame;
using System;
using System.Windows.Forms;

// Starta applikationen och spelfönster
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