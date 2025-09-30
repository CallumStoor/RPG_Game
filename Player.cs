using System;



public class Player
{
    public int PlayerHealth = 10;
    public int PlayerIntellegence = 1;
    public int PlayerIntimidation = 1;
    public int PlayerCharisma = 1;
    public int PlayerSneak = 1;
    public int PlayerGold = 1;
    public string PlayerName = "Dweller";

    public Player(string playerName, int playerHealth, int playerIntellegence, int playerIntimidaton, int playerCharisma, int playerSneak, int playerGold)
    {
        PlayerName = playerName;
        PlayerHealth = playerHealth;
        PlayerCharisma = playerCharisma;
        PlayerIntellegence = playerIntellegence;
        PlayerIntimidation = playerIntimidaton;
        PlayerSneak = playerSneak;
        PlayerGold = playerGold;
    }

    public int StatIncrease(int Stat, string StatType) // display and increase stats 
    {
        Stat += 1;
        ScrollText($"Your {StatType} has increased by 1 Level");
        ScrollText($"You are now on {StatType} level: {Stat}");

        return Stat;
    }


    void ScrollText(string text) // scrolls through text giving it a nice animation
    {
        Console.WriteLine("");
        for (int i = 0; i < text.Length; i++)
        {
            Thread.Sleep(30);
            Console.Write(text[i]);

        }
        Thread.Sleep(1000);
    }
}
