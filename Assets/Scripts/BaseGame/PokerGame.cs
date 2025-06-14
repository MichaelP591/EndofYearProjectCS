using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using BaseGame;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.XR;

public class PokerGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int humanPlayerCount;
    public int algorithmPlayerCount;
    public Round round = new Round();
    List<Player> players;
    List<Pot> pots = new List<Pot>();
    PokerCard[,] deck = new PokerCard[4, 13];
    string[] suits = { "diamond", "spade", "heart", "club" };
    private readonly System.Random random = new System.Random();
    public int currentBet = 0;
    public enum BettingRound
    {
        PreFlop, Flop, Turn, River
    }
    public BettingRound bettingRound { get; set; }
    void Start()
    {
        for (int i = 0; i < humanPlayerCount; i++) { players.Add(new HumanPlayer()); }
        for (int i = 0; i < algorithmPlayerCount; i++) { players.Add(new AlgorithmPlayer(round)); }
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
        Instantiate(GameObject.Find("Board").GetComponent<BoardScript>().CardParent, GameObject.Find("Board").GetComponent<BoardScript>().CommunityCardsLayout.transform);
        BoardScript board = GameObject.Find("Board").GetComponent<BoardScript>();
        board.DealCommunityCards();
    }
    private void ProcessBettingRound()
    {
        currentBet = 0;
        bool bettingComplete = false;

        while (!bettingComplete)
        {
            bettingComplete = true;

            foreach (Player player in players)
            {
                if (!player.IsTurn) continue;  // Skip players who have folded or are all-in

                // Get player's betting decision
                int minimumBet = currentBet - player.CurrentBet;  // How much more they need to call
                player.MakeBet(bettingRound);  // This will trigger the player's UI or AI decision


                // Handle the player's decision (this will be received through events/callbacks)
                if (player.LastAction == Player.PlayerAction.Raise)
                {
                    currentBet = player.CurrentBet;
                    bettingComplete = false;  // Need another round if someone raises
                }
            }
        }
    }
    void CreatePots(List<Player> players, List<Pot> pots)
    {
        // Get distinct bet amounts in ascending order
        var allInAmounts = players.Where(p => p.CurrentBet > 0).Select(p => p.Stack).Distinct().OrderBy(b => b).ToList();

        int previous = 0;

        foreach (int level in allInAmounts) {
            int potSize = 0;
            var eligible = new HashSet<Player>();

            foreach (var player in players)
            {
                int contribution = Math.Min(level - previous, player.CurrentBet);
                if (contribution > 0) {
                    potSize += contribution;
                    player.CurrentBet -= contribution;
                    if (player.LastAction != Player.PlayerAction.Fold) eligible.Add(player);
                }
            }
            if (potSize > 0) pots.Add(new Pot { Amount = potSize, EligiblePlayers = eligible });
            previous = level;
        }
    }
    void ResolvePots(List<Pot> pots)
    {
        foreach (Pot pot in pots)
        {
            var contenders = pot.EligiblePlayers.Where(p => !(p.LastAction != Player.PlayerAction.Fold)).ToList();
            if (contenders.Count == 0) continue;

            var winners = round.DeclareWinner(contenders);
            foreach (Player winner in winners) winner.Stack += pot.Amount;
        }
    }
    bool AnyoneIsAllIn()
    {
        int count = 0;
        foreach (var player in players) if (player.LastAction == Player.PlayerAction.AllIn) count++; 
        return count > 0;
    }
    void PreFlop() //betting round before any cards are revealed
    {
        bettingRound = BettingRound.PreFlop;
        ProcessBettingRound();
        round.NextCard();
        round.NextCard();
        round.NextCard();
    }
    void Flop() //betting round after first three cards are revealed
    {
        bettingRound = BettingRound.Flop;
        ProcessBettingRound();
        round.NextCard();
    }
    void Turn() //betting round before final card reveal
    {
        bettingRound = BettingRound.Turn;
        ProcessBettingRound();
        round.NextCard();
    }
    void River() //final betting round
    {
        bettingRound = BettingRound.River;
        ProcessBettingRound();
        if (AnyoneIsAllIn()) CreatePots(players, pots);
    }
    void ShowDown() // reveal all cards and declare winner and split pot
    {
        ResolvePots(pots);
        foreach (var player in players) {
            player.Stack = 0;
            player.CurrentBet = 0;
        }
        pots.Clear();
    }
}
