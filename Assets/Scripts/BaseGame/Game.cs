using System.Collections.Generic;
using BaseGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Round;
    public int humanPlayerCount;
    public int algorithmPlayerCount;
    Round round = new Round();
    List<Player> players;


    void Start()
    {
        for (int i = 0; i < humanPlayerCount; i++) { players.Add(new HumanPlayer()); }
        for (int i = 0; i < algorithmPlayerCount; i++) { players.Add(new AlgorithmPlayer()); }
        foreach (Player player in players) { round.AddPlayer(player); }
        
    }

    // Update is called once per frame
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
