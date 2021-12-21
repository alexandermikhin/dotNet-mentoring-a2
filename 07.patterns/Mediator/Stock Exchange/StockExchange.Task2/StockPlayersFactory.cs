namespace StockExchange.Task2
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var players = new Players();
            players.RedSocks = new RedSocks("RedSocks", players);
            players.Blossomers = new Blossomers("Blossomers", players);
            players.RossStones = new RossStones("RossStones", players);

            return players;

        }
    }
}
