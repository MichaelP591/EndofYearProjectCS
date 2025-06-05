using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using BaseGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Game : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int humanPlayerCount;
    public int algorithmPlayerCount;
    Round round = new Round();
    List<Player> players;
    PokerCard[,] deck = new PokerCard[4, 13];
    string[] suits = { "diamond", "spade", "heart", "club" };
    private readonly System.Random random = new System.Random();
    void Start()
    {
        for (int i = 0; i < humanPlayerCount; i++) { players.Add(new HumanPlayer()); }
        for (int i = 0; i < algorithmPlayerCount; i++) { players.Add(new AlgorithmPlayer()); }
        foreach (Player player in players) { round.AddPlayer(player); }
        for (int i = 0; i < deck.GetLength(0); i++)
        {
            for (int j = 0; j < deck.GetLength(1); i++)
            {
                deck[i, j] = new PokerCard(j + 2, suits[i]);
            }
        }
        foreach (Player player in players)
        {
            PokerCard[] hand = new PokerCard[2];
            do
            {
                int randomSuit = random.Next(0, 4);
                int randomRank = random.Next(0, 13);
                if (deck[randomSuit, randomRank] != null)
                {
                    hand[0] = deck[randomSuit, randomRank];
                    deck[randomSuit, randomRank] = null;
                    break;
                }
            } while (true);
            do
            {
                int randomSuit = random.Next(0, 4);
                int randomRank = random.Next(0, 13);
                if (deck[randomSuit, randomRank] != null)
                {
                    hand[1] = deck[randomSuit, randomRank];
                    deck[randomSuit, randomRank] = null;
                    break;
                }
            } while (true);
            player.Hand = hand;
        }
        PokerCard[] houseHand = new PokerCard[5];
        for (int i = 0; i < 5; i++)
        {
            do
            {
                int randomSuit = random.Next(0, 4);
                int randomRank = random.Next(0, 13);
                if (deck[randomSuit, randomRank] != null)
                {
                    houseHand[i] = deck[randomSuit, randomRank];
                    deck[randomSuit, randomRank] = null;
                    break;
                }
            } while (true);
        }
    }

    void Update()
    {
        
    }
    void PreFlop() //betting round before any cards are revealed
    {

        round.NextCard();
        round.NextCard();
        round.NextCard();
    }
    void Flop() //betting round after first three cards are revealed
    {
        round.NextCard();
    }
    void Turn() //betting round before final card reveal
    {
        round.NextCard();
    }
    void River() //final betting round
    {

    }
    void ShowDown() // reveal all cards and declare winner and split pot
    {
        
    }
}
