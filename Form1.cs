/* DT071G Programmering i C#.NET Projektuppgift. Åsa Lindskog sali1502@student.miun.se */
/* En memoryspel med topplista baserat på antal försök att matcha bildpar. */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame
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
            InitializeComponent(); // Initierar konfiguration av UI-komponenter
        }

        // Starta spelet
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
            CreateButtons(); // Skapa knappar - klickbar baksida för att vända kort (bild)
            Shuffle(images); // Blanda kort (bilder) slumpmässigt
        }

        // Ladda bilder för spelet
        private void LoadImages()
        {
            try
            {
                // Ladda in bilder för varje kortpar
                images[0] = Image.FromFile("Images/earth.jpg");
                images[1] = Image.FromFile("Images/venus.jpg");
                images[2] = Image.FromFile("Images/mars.jpg");
                images[3] = Image.FromFile("Images/jupiter.jpg");
                images[4] = Image.FromFile("Images/saturn.jpg");
                images[5] = Image.FromFile("Images/uranus.jpg");
                images[6] = Image.FromFile("Images/neptune.jpg");
                images[7] = Image.FromFile("Images/meteor.jpg");

                // Skapa par genom att duplicera bilder
                for (int i = 0; i < NUM_PAIRED_CARDS; i++)
                {
                    images[i + NUM_PAIRED_CARDS] = images[i];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ladding av bilder: " + ex.Message);
            }
        }

        // Skapa knappar för korten som gör att de kan vändas
        private void CreateButtons()
        {
            int size = 100;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new Button
                {
                    Size = new Size(size, size),
                    Location = new Point((i % 4) * size, (i / 4) * size + 50), // Placering i ett 4 x 4-rutnät
                    BackColor = Color.LightSteelBlue,
                    Tag = i // Varje knapp får ett index för att kunna identifieras
                };
                buttons[i].Click += Button_Click; // Kopplar en klickhändelse till knappen
                Controls.Add(buttons[i]); // Lägger till knappen i formuläret
            }
        }

        // Blanda bilder med Fisher-Yates algoritmen
        private void Shuffle(Image[] array)
        {
            Random random = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Image temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        // Metod för när en knapp (ett kort/bild) klickas
        private async void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int index)
            {
                // Det första valda kortet
                if (firstChoiceIndex == -1)
                {
                    firstChoiceIndex = index;
                    clickedButton.BackgroundImage = images[index];
                    clickedButton.BackgroundImageLayout = ImageLayout.Zoom;
                    clickedButton.Enabled = false; // Inaktivera knappen temporärt
                }
                // Det andra valda kortet
                else if (secondChoiceIndex == -1 && index != firstChoiceIndex)
                {
                    secondChoiceIndex = index;
                    clickedButton.BackgroundImage = images[index];
                    clickedButton.BackgroundImageLayout = ImageLayout.Zoom;
                    clickedButton.Enabled = false;
                    turns++; // Öka antal försök

                    // Vänta för att visa båda korten
                    await Task.Delay(1000);

                    // Kontrollera om korten är ett matchande par
                    CheckForMatch();
                }
            }
        }

        // Kontrollera om de valda korten matchar
        private void CheckForMatch()
        {
            if (images[firstChoiceIndex] != images[secondChoiceIndex])
            {
                // Om ingen match, vänd tillbaka korten
                buttons[firstChoiceIndex].BackgroundImage = null;
                buttons[secondChoiceIndex].BackgroundImage = null;
                buttons[firstChoiceIndex].Enabled = true;
                buttons[secondChoiceIndex].Enabled = true;
            }
            else
            {
                // Om match, håll korten dolda
                buttons[firstChoiceIndex].Visible = false;
                buttons[secondChoiceIndex].Visible = false;
            }

            ClearChoices(); // Rensa val för nästa omgång

            // Kontrollera om alla par är hittade
            if (AllMatched() && playerNameTextBox != null)
            {
                highScores.Add((playerNameTextBox.Text, turns)); // Lägg till resultat i topplista
                DisplayHighScores(); // Visa uppdaterad topplista
                MessageBox.Show($"Grattis! Du har hittat alla par på {turns} försök.");
                EnableScoreButtons(); // Visa knappar för topplista och spela igen
            }
        }

        // Rensa index för valda kort för en ny omgång
        private void ClearChoices()
        {
            firstChoiceIndex = -1;
            secondChoiceIndex = -1;
        }

        // Kontrollera om alla par har hittats
        private bool AllMatched()
        {
            return buttons.All(button => !button.Visible);
        }

        // Återställ spelet för en ny omgång
        private void ResetGame()
        {
            foreach (Button button in buttons)
            {
                button.Visible = true;
                button.BackgroundImage = null;
                button.Enabled = true;
            }
            Shuffle(images); // Blanda korten igen
            turns = 0;
            ClearChoices();
        }

        // Visa topplistan med namn ordnat efter minst antal försök
        private void DisplayHighScores()
        {
            if (highScoreLabel == null) return;
            highScoreLabel.Text = "Topplista:\n";
            foreach (var score in highScores.OrderBy(s => s.Turns))
            {
                highScoreLabel.Text += $"{score.PlayerName}: {score.Turns} försök\n";
            }
            highScoreLabel.Visible = true; // Visa topplistan
        }

        // Rensa topplistan
        private void ClearScoresButton_Click(object sender, EventArgs e)
        {
            highScores.Clear();
            if (highScoreLabel != null)
            {
                highScoreLabel.Text = "Topplista:\n";
            }
            MessageBox.Show("Topplistan har rensats.");
        }

        // Återställ spelet
        private void PlayAgainButton_Click(object sender, EventArgs e)
        {
            ResetGame();
            if (playerNameTextBox != null)
                playerNameTextBox.Enabled = true;

            if (startButton != null)
                startButton.Visible = false;

            if (highScoreLabel != null)
                highScoreLabel.Visible = false;

            if (clearScoresButton != null)
                clearScoresButton.Visible = false;

            if (playAgainButton != null)
                playAgainButton.Visible = false;
        }

        // Visa knappar för topplista och spela igen efter avslutat spel
        private void EnableScoreButtons()
        {
            if (highScoreLabel != null) highScoreLabel.Visible = true;
            if (clearScoresButton != null) clearScoresButton.Visible = true;
            if (playAgainButton != null) playAgainButton.Visible = true;
        }
    }
}