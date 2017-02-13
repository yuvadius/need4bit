using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {
    public int nameLength;
    public int maxLeaderboardCount;//Max players to show on scoreboard

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "/" + PhotonNetwork.GetPing().ToString());
        if (Matchmaker.startedGame)
        {
            GUI.contentColor = Color.yellow;
            //GUILayout.Label("Score: " + PhotonNetwork.player.);
            KeyValuePair<string, int>[] leaderboard = GetLeaderboard();
            //Sort array by descending order of score
            Array.Sort(leaderboard, delegate (KeyValuePair<string, int> x, KeyValuePair<string, int> y)
            {
                return y.Value.CompareTo(x.Value);
            });
            for (int i = 0; i < leaderboard.Length && i < maxLeaderboardCount; i++)
            {
                string name;
                if (leaderboard[i].Key.Length >= nameLength)
                    name = leaderboard[i].Key.Substring(0, nameLength);
                else
                    name = leaderboard[i].Key + (new string(' ', nameLength - leaderboard[i].Key.Length));
                GUILayout.Label("#" + (i + 1) + "    " + name + ": " + leaderboard[i].Value);
            }
        }
    }

    static KeyValuePair<string, int>[] GetLeaderboard()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        KeyValuePair<string, int>[] leaderboard = new KeyValuePair<string, int>[players.Length];
        for (int i = 0; i < players.Length; i++)
            leaderboard[i] = new KeyValuePair<string, int>(players[i].name, players[i].GetScore());
        return leaderboard;
    }
}
