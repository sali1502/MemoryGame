﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace MemoGameProjekt
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Komponenter för UI (User Interface)
        private TextBox playerNameTextBox; // Inputfält för spelarens namn
        private Button startButton;
        private Label highScoreLabel; // Visa topplista
        private Button clearScoresButton; //Rensa topplista
        private Button playAgainButton; // Starta om spelet

        // Metod för att frigöra resurser som används av komponenter
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Metod för att initiera och konfigurera UI (User Interface)-komponenter
        private void InitializeComponent()
        {
            SuspendLayout();

            // Inputfält för spelarens namn
            playerNameTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Width = 150,
                PlaceholderText = "Ange ditt namn"
            };
            Controls.Add(playerNameTextBox);

            startButton = new Button
            {
                Text = "Börja Spela",
                Location = new Point(170, 10),
                Width = 100 
            };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton); 

            // Topplista
            highScoreLabel = new Label
            {
                Location = new Point(10, 50),
                Size = new Size(400, 460),
                AutoSize = true,
                Text = "Topplista:\n",
                Visible = false
            };
            Controls.Add(highScoreLabel);

            // Knapp för att rensa topplista
            clearScoresButton = new Button
            {
                Text = "Rensa Topplista", 
                Location = new Point(10, 360), 
                Width = 120, 
                Visible = false 
            };
            clearScoresButton.Click += ClearScoresButton_Click; 
            Controls.Add(clearScoresButton); 

            // Knapp för att spela en ny omgång
            playAgainButton = new Button
            {
                Text = "Spela igen", 
                Location = new Point(170, 360),
                Width = 120,
                Visible = false
            };
            playAgainButton.Click += PlayAgainButton_Click; 
            Controls.Add(playAgainButton); 

            // inställningar för Form1 (spelfönstret)
            ClientSize = new Size(400, 460);
            Name = "Form1";
            Text = "Välkommen till Memory Game!";
            ResumeLayout(false);
        }
    }
}
