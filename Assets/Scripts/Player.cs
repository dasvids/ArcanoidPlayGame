[System.Serializable]
public class Player
{
    public string Name;
    public int Score;

    public Player(string name, int score)
    {
        Name = name;
        Score = score;
    }
    public override string ToString()
    {
        return $"{this.Name} {this.Score}";
    }
    public string toUi()
    {
        return $"Name: {this.Name}, Score: {this.Score}";
    }
}