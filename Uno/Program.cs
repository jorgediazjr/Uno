using System;
using System.Collections;

namespace Uno
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayList unoDeck = CreateCompleteUnoDeck();
            unoDeck = ShuffleDeck(unoDeck);

            Stack unoDeckStack = ConvertDeckToStack(unoDeck);

            ArrayList opponentDeck = GetStartingDeck(unoDeckStack);
            ArrayList userDeck = GetStartingDeck(unoDeckStack);

            PlayGame(unoDeckStack, unoDeck, opponentDeck, userDeck);

            Console.ReadLine();
        }

        private static ArrayList CreateCompleteUnoDeck()
        {
            int numberOfZeroCards = 1;
            int numberOfNonzeroCards = 18;
            string[] colors = { "red", "blue", "green", "yellow" };
            int numberOfColors = colors.Length; // red, green, blue, yellow
            int colorIndex = 0;

            ArrayList deck = new ArrayList(); // this is for the complete deck of cards

            // this is to put the four zeros in
            for (int i = 0; i < numberOfZeroCards * numberOfColors; i++)
            {
                deck.Add(new Card(colors[i], 0));
            }

            // this is for the rest of the deck
            int cardNum = 1;
            for (int i = numberOfZeroCards * numberOfColors;
                     i < (numberOfZeroCards + numberOfNonzeroCards) * numberOfColors;
                     i += 2)
            {
                if (cardNum == 10)
                {
                    cardNum = 1;
                    colorIndex++;
                }
                deck.Insert(i, new Card(colors[colorIndex], cardNum));
                deck.Insert(i + 1, new Card(colors[colorIndex], cardNum));
                cardNum++;
            }

            return deck;
        }

        private static ArrayList ShuffleDeck(ArrayList unoDeck)
        {
            Random random = new Random();

            for (int n = unoDeck.Count - 1; n > 0; --n)
            {
                int k = random.Next(n + 1);
                Card temp = (Card) unoDeck[n];
                unoDeck[n] = unoDeck[k];
                unoDeck[k] = temp;
            }

            /*
            for (int i = 0; i < unoDeck.Count; i++)
            {
                Card c = (Card)unoDeck[i];
                Console.WriteLine(c.ToString());
            }
            */

            return unoDeck;
        }

        private static Stack ConvertDeckToStack(ArrayList unoDeck)
        {
            Stack unoDeckStack = new Stack();

            for (int i = 0; i < unoDeck.Count; i++)
            {
                unoDeckStack.Push(unoDeck[i]);
            }

            return unoDeckStack;
        }

        private static ArrayList GetStartingDeck(Stack unoDeckStack)
        {
            ArrayList startingDeck = new ArrayList();

            int numOfInitialCards = 7;

            for (int i = 0; i < numOfInitialCards; i++)
            {
                startingDeck.Add(unoDeckStack.Pop());
            }

            return startingDeck;
        }

        private static void PlayGame(Stack gameDeck, ArrayList unoDeck,
                             ArrayList opponentDeck, ArrayList userDeck)
        {
            bool gameOver = false;
            Card topCard = (Card) gameDeck.Pop();

            while (!gameOver)
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("                                  ");
                Console.WriteLine("            " + topCard.ToString() + "             ");
                Console.WriteLine("                                  ");
                Console.WriteLine("                                  ");
                Console.WriteLine("                                  ");
                Console.WriteLine("         Your cards:                   ");

                // print out the user's cards
                for (int i = 0; i < userDeck.Count; i++)
                {
                    Card userCard = (Card) userDeck[i];
                    Console.WriteLine(i + ": " + userCard.ToString());
                }
                Console.WriteLine("                                  ");

                bool cardPlaced = false;
                bool cardDrawn = false;
                bool possiblePlacement = false;
                while (!cardPlaced && !cardDrawn)
                {
                    // i have to add this check
                    if (gameDeck.Count == 0)
                    {
                        gameOver = true;
                        break;
                    }

                    if (userDeck.Count == 1)
                    {
                        Console.WriteLine("You say UNO!");
                    }

                    if (opponentDeck.Count == 1)
                    {
                        Console.WriteLine("Opponent says UNO!");
                    }
                    for (int i = 0; i < userDeck.Count; i++)
                    {
                        Card c = (Card)userDeck[i];
                        if (c.Color == topCard.Color || c.Number == topCard.Number)
                        {
                            possiblePlacement = true;
                            break;
                        }
                    }

                    if (possiblePlacement)
                    {
                        bool goodInput = false;
                        Console.WriteLine("You have a possibility of choosing a card: ");
                        while (!goodInput)
                        {
                            try
                            {
                                int choice = Convert.ToInt32(Console.ReadLine());
                                goodInput = true;
                                if (choice >= 0 && choice <= userDeck.Count - 1)
                                {
                                    Card userChoice = (Card)userDeck[choice];
                                    if (userChoice.Color == topCard.Color || userChoice.Number == topCard.Number)
                                    {
                                        topCard = (Card)userChoice;
                                        userDeck.RemoveAt(choice);
                                        cardPlaced = true;
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }   
                    }
                    else
                    {
                        Console.WriteLine("You are drawing a card now...");
                        Console.ReadLine();
                        userDeck.Add(gameDeck.Pop()); // removing card from game deck
                        cardDrawn = true;
                    }
                }

                // check if your deck is done
                if (userDeck.Count == 0)
                {
                    Console.WriteLine("Congratulations ... You are the winner !!!");
                    break;
                }

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                Console.WriteLine("Opponent's turn");

                cardPlaced = false;
                cardDrawn = false;

                while (!cardPlaced && !cardDrawn)
                {
                    // i have to add this check
                    if (gameDeck.Count == 0)
                    {
                        gameOver = true;
                        break;
                    }
                    for (int i = 0; i < opponentDeck.Count; i++)
                    {
                        Card c = (Card)opponentDeck[i];
                        if (c.Color == topCard.Color || c.Number == topCard.Number)
                        {
                            topCard = (Card)c;
                            opponentDeck.RemoveAt(i);
                            cardPlaced = true;
                            Console.WriteLine("Opponent placed card...");
                            break;
                        }
                    }

                    if (!cardPlaced)
                    {
                        Console.WriteLine("Opponent drew card...");
                        opponentDeck.Add(gameDeck.Pop()); // removing card from game deck
                        cardDrawn = true;
                    }
                }

                // checks if opponent's deck is dead
                if (opponentDeck.Count == 0)
                {
                    Console.WriteLine("I am sorry ... You are the loser !!!");
                    break;
                }
            }

            if (opponentDeck.Count > 0 && userDeck.Count > 0)
            {
                Console.WriteLine("It is a TIE, no one won!");
            }
        }

    }
}
