using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Match3Solver
{
    public partial class CardPicker : Form
    {
        public Card SelectedCard { get; private set; }

        public CardPicker()
        {
            InitializeComponent();
            ShowCards();
        }

        private void ShowCards()
        {
            Array suits = Enum.GetValues(typeof(SuitType));
            Array ranks = Enum.GetValues(typeof(RankType));

            PictureBox[] pictureBoxes = Controls.OfType<PictureBox>().ToArray();
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                var (rank, suit) = ParsePictureBoxName(pictureBox);
                var card = new Card((SuitType)Enum.Parse(typeof(SuitType), suit), (RankType)Enum.Parse(typeof(RankType), rank));
                pictureBox.Image = Image.FromFile(card.ToImageLocation());
                pictureBox.Click += PictureBox_Click;
                pictureBox.MouseHover += PictureBox_MouseHover;
            }
        }

        private void PictureBox_MouseHover(object? sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                var (rank, suit) = ParsePictureBoxName(pictureBox);
                var card = new Card((SuitType)Enum.Parse(typeof(SuitType), suit), (RankType)Enum.Parse(typeof(RankType), rank));
                toolTip1.SetToolTip(pictureBox, card != null ? card.ToShortString() : "Empty");
            }
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                SelectedCard = SelectCard(pictureBox);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        private (string, string) ParsePictureBoxName(PictureBox pictureBox)
        {
            // Name is in the format "pictureBox_rank_suit"
            return (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pictureBox.Name.Split('_')[1]), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pictureBox.Name.Split('_')[2]));
        }

        private Card SelectCard(PictureBox pictureBox)
        {
            var (rank, suit) = ParsePictureBoxName(pictureBox);
            return new Card((SuitType)Enum.Parse(typeof(SuitType), suit), (RankType)Enum.Parse(typeof(RankType), rank));
        }
    }
}
