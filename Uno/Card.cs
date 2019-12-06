using System;
namespace Uno
{
    public class Card
    {
        private String color;
        private int number;

        public Card()
        {
        }

        public Card(String color, int number)
        {
            this.Color = color;
            this.Number = number;
        }

        public string Color
        {
            get => color;
            set => color = value;
        }

        public int Number
        {
            get => number;
            set => number = value;
        }

        public string ToString()
        {
            return color + " / " + number;
        }
    }
}
