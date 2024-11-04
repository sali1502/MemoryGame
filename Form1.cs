using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoGameProjekt
{
    public partial class Form1 : Form
    {
        // Konstanter och variabler
        private const int NUM_PAIRED_CARDS = 8; // Åtta par kort (bilder)
        private Button[] buttons = new Button[NUM_PAIRED_CARDS * 2]; // Array av osynliga "knappar" som gör baksidan av korten (bilderna) klickbara
        private Image[] images = new Image[NUM_PAIRED_CARDS * 2]; // Array för korten (bilderna)
        private int firstChoiceIndex = -1; // Index för det först valda kortet (bilden)
        private int secondChoiceIndex = -1; // Index för det andra valda kortet (bilden)
        private int turns = 0; // Räknare för antalet försök

        // Lista för topplista med spelares namn och antal försök (lagras till nästa spelomgång men ej vid stängning av appen)
        private List<(string PlayerName, int Turns)> highScores = new List<(string, int)>();

        public Form1()
        {
            InitializeComponent(); // Initierar konfiguration UI-komponenter från Form1.Designer.cs
        }

        // Startar spelet
        private void StartButton_Click(object sender, EventArgs e)
        {
            InitializeGame();

            if (playerNameTextBox != null)
                playerNameTextBox.Enabled = false;

            if (startButton != null)
                startButton.Visible = false;
        }

        // Initiera spelet med att ladda bilder, skapa knappar och blanda kort (bilder)
        private void InitializeGame()
        {
            LoadImages(); // Ladda kort (bilder)
            CreateButtons(); // Skapa knappar
            Shuffle(images); // Blanda kort (bilder) slumpmässigt
        }
