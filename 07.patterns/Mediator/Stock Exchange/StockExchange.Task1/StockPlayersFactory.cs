namespace StockExchange.Task1
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var players = new Players();
            players.RedSocks = new RedSocks(players);
            players.Blossomers = new Blossomers(players);

            return players;
        }
    }
}
