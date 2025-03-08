namespace Match3Solver
{
    public partial class Main : Form
    {
        // Grid to hold the cards on the board
        private Card[,] cardGrid = new Card[5, 8];

        // Array to hold the player's cards
        private Card[] playerCards = new Card[4];

        public Main()
        {
            InitializeComponent();
            InitializePictureBoxes();
        }
        private void ResetPictureBoxBorders()
        {
            foreach (Control control in groupBox1.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Paint -= PictureBox_Paint;
                    pictureBox.Invalidate();
                }
            }
            foreach (Control control in groupBox2.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Paint -= PictureBox_Paint;
                    pictureBox.Invalidate();
                }
            }
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                using (Pen pen = new Pen(Color.Yellow, 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pictureBox.ClientSize.Width - 1, pictureBox.ClientSize.Height - 1);
                }
            }
        }
        // Initialize event handlers for PictureBox controls
        private void InitializePictureBoxes()
        {
            foreach (Control control in groupBox1.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += PictureBoxOnBoard_Click;
                    pictureBox.MouseHover += PictureBoxOnBoard_Hover;
                }
            }
            foreach (Control control in groupBox2.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Click += PictureBoxPlayerHand_Click;
                    pictureBox.MouseHover += PictureBoxPlayerHand_Hover;
                }
            }
        }

        // Event handler for clicking a PictureBox on the board
        private void PictureBoxOnBoard_Click(object? sender, EventArgs e)
        {
            using var cardPicker = new CardPicker();
            if (cardPicker.ShowDialog() == DialogResult.OK)
            {
                Card selectedCard = cardPicker.SelectedCard;
                if (sender is PictureBox pictureBox)
                {
                    pictureBox.Image = Image.FromFile(selectedCard.ToImageLocation());
                    var position = ParsePositionFromName2D(pictureBox.Name);
                    cardGrid[position.Item1, position.Item2] = selectedCard;
                    DisplayCardMoves();
                }
            }
        }

        // Event handler for clicking a PictureBox in the player's hand
        private void PictureBoxPlayerHand_Click(object? sender, EventArgs e)
        {
            using var cardPicker = new CardPicker();
            if (cardPicker.ShowDialog() == DialogResult.OK)
            {
                Card selectedCard = cardPicker.SelectedCard;
                if (sender is PictureBox pictureBox)
                {
                    pictureBox.Image = Image.FromFile(selectedCard.ToImageLocation());
                    var position = ParsePositionFromName(pictureBox.Name);
                    if (position >= 0 && position < playerCards.Length)
                    {
                        playerCards[position] = selectedCard;
                    }
                    DisplayCardMoves();
                }
            }
        }

        // Event handler for hovering over a PictureBox on the board
        private void PictureBoxOnBoard_Hover(object? sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                var position = ParsePositionFromName2D(pictureBox.Name);
                var card = cardGrid[position.Item1, position.Item2];
                toolTip1.SetToolTip(pictureBox, card != null ? card.ToShortString() : "Empty");
            }
        }

        // Event handler for hovering over a PictureBox in the player's hand
        private void PictureBoxPlayerHand_Hover(object? sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                var position = ParsePositionFromName(pictureBox.Name);
                var card = playerCards[position];
                toolTip1.SetToolTip(pictureBox, card != null ? card.ToShortString() : "Empty");
            }
        }

        // Parse the position from the name of a PictureBox on the board
        private static Tuple<int, int> ParsePositionFromName2D(string name)
        {
            var parts = name.Split('_');
            int row = int.Parse(parts[1]);
            int col = int.Parse(parts[2]);
            return Tuple.Create(row, col);
        }

        // Parse the position from the name of a PictureBox in the player's hand
        private static int ParsePositionFromName(string name)
        {
            return int.Parse(name.Split('_')[1]);
        }

        // Calculate the score for a line of 3 cards
        private static int CalculateLineScore(Card[] cards)
        {
            if (cards.Length != 3) return 0;

            // Check for null cards and return 0 if any are found
            if (cards[0] == null || cards[1] == null || cards[2] == null) return 0;

            bool isTrio = cards[0].Rank == cards[1].Rank && cards[1].Rank == cards[2].Rank;
            bool isPair = cards[0].Rank == cards[1].Rank || cards[1].Rank == cards[2].Rank || cards[0].Rank == cards[2].Rank;
            bool isColor = cards[0].Suit == cards[1].Suit && cards[1].Suit == cards[2].Suit;
            bool isSequence = IsSequence(cards);
            bool isPureSequence = isColor && isSequence;

            if (isTrio) return 100;
            if (isPureSequence) return 60;
            if (isSequence) return 40;
            if (isColor) return 20;
            if (isPair) return 10;

            return 0;
        }

        // Check if the cards form a sequence
        private static bool IsSequence(Card[] cards)
        {
            var sortedRanks = cards.Select(c => c.Rank).OrderBy(rank => rank).ToArray();
            return sortedRanks[0] + 1 == sortedRanks[1] && sortedRanks[1] + 1 == sortedRanks[2];
        }

        // Get the description of the line formed by the cards
        private static string GetLineDescription(Card[] cards)
        {
            if (cards.Length != 3) return string.Empty;

            // Check for null cards and return 0 if any are found
            if (cards[0] == null || cards[1] == null || cards[2] == null) return string.Empty;

            bool isTrio = cards[0].Rank == cards[1].Rank && cards[1].Rank == cards[2].Rank;
            bool isPair = cards[0].Rank == cards[1].Rank || cards[1].Rank == cards[2].Rank || cards[0].Rank == cards[2].Rank;
            bool isColor = cards[0].Suit == cards[1].Suit && cards[1].Suit == cards[2].Suit;
            bool isSequence = IsSequence(cards);
            bool isPureSequence = isColor && isSequence;

            if (isTrio) return "trio";
            if (isPureSequence) return "pure sequence";
            if (isSequence) return "sequence";
            if (isColor) return "color";
            if (isPair) return "pair";

            return string.Empty;
        }

        private void DisplayCardMoves() // Needed
        {
            ResetPictureBoxBorders();
            listBox1.Items.Clear();
            var moves = new List<(int score, string description)>();

            for (int i = 0; i < playerCards.Length; i++)
            {
                var playerCard = playerCards[i];
                if (playerCard != null)
                {
                    for (int row = 0; row < cardGrid.GetLength(0); row++)
                    {
                        for (int col = 0; col < cardGrid.GetLength(1); col++)
                        {
                            if (cardGrid[row, col] == null)
                            {
                                // Check for neighboring cards before placing the card
                                if (HasNeighbor(row, col))
                                {
                                    cardGrid[row, col] = playerCard;

                                    List<string> comboDescriptions = [];
                                    int totalScore = 0;
                                    int scoringConditions = 0;

                                    // Check all possible directions
                                    foreach (var (rowDir, colDir) in new (int, int)[] { (0, 1), (1, 0), (1, 1), (-1, 1) })
                                    {
                                        var (canFormLine, lineScore, description) = CheckLineWithDetails(row, col, rowDir, colDir);
                                        if (canFormLine)
                                        {
                                            totalScore += lineScore;
                                            scoringConditions++;
                                            comboDescriptions.Add(description);
                                        }
                                    }

                                    // Apply multiplier if multiple conditions exist
                                    if (scoringConditions > 1)
                                    {
                                        totalScore *= scoringConditions;
                                    }

                                    // Add the move to the list of moves
                                    if (scoringConditions > 0)
                                    {
                                        string descriptionText = scoringConditions > 1 ?
                                            $"Combo: {string.Join(", ", comboDescriptions)}" : comboDescriptions[0];

                                        moves.Add((totalScore, $"Move {playerCard.ToShortString()} to (row: {row + 1}, column: {col + 1}) for {totalScore} points ({descriptionText})"));
                                    }

                                    // Reset the board state
                                    cardGrid[row, col] = null;
                                }
                            }
                        }
                    }
                }
            }

            // Sort the moves by score in descending order
            moves.Sort((a, b) => b.score.CompareTo(a.score));

            // Add the sorted moves to the listBox1
            foreach (var (score, description) in moves)
            {
                listBox1.Items.Add(description);
            }

            // Highlight the best move
            if (moves.Count > 0)
            {
                var bestMove = moves[0];
                int col = int.Parse(bestMove.description.Split("column: ")[1].Split(")")[0]) -1;
                int row = int.Parse(bestMove.description.Split("row: ")[1].Split(",")[0]) -1;
                var pictureBoxName = $"pictureBox_{row}_{col}";
                var pictureBox = groupBox1.Controls.OfType<PictureBox>().FirstOrDefault(pb => pb.Name == pictureBoxName);
                if (pictureBox != null)
                {
                    pictureBox.Paint += PictureBox_Paint;
                    pictureBox.Invalidate();
                }
            }
        }

        // Check if placing a card at the given position can form a line in the specified direction and return details
        private (bool, int, string) CheckLineWithDetails(int row, int col, int rowDir, int colDir) // Needed
        {
            var cards = new Card?[3];
            for (int i = -2; i <= 0; i++)
            {
                bool outOfBounds = false;
                for (int j = 0; j < 3; j++)
                {
                    int r = row + (i + j) * rowDir;
                    int c = col + (i + j) * colDir;
                    if (r < 0 || r >= cardGrid.GetLength(0) || c < 0 || c >= cardGrid.GetLength(1))
                    {
                        outOfBounds = true;
                        break;
                    }
                    cards[j] = cardGrid[r, c];
                }
                if (!outOfBounds && cards[0] != null && cards[1] != null && cards[2] != null)
                {
                    int score = CalculateLineScore(cards!);
                    if (score > 0)
                    {
                        string description = GetLineDescription(cards!);
                        return (true, score, description);
                    }
                }
            }
            return (false, 0, string.Empty);
        }

        // Check if a position has a neighboring card
        private bool HasNeighbor(int row, int col) // Needed
        {
            var directions = new (int rowDir, int colDir)[] { (0, 1), (1, 0), (0, -1), (-1, 0), (1, 1), (-1, -1), (1, -1), (-1, 1) };
            foreach (var (rowDir, colDir) in directions)
            {
                int r = row + rowDir;
                int c = col + colDir;
                if (r >= 0 && r < cardGrid.GetLength(0) && c >= 0 && c < cardGrid.GetLength(1) && cardGrid[r, c] != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cardGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cardGrid.GetLength(1); j++)
                {
                    cardGrid[i, j] = null;
                }
            }

            Array.Clear(playerCards);
            listBox1.Items.Clear();
            ResetPictureBoxBorders();

            foreach (Control control in groupBox1.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Image = null;
                }
            }
            foreach (Control control in groupBox2.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Image = null;
                }
            }
        }
    }
}