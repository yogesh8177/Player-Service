namespace Player_Service.Models
{
    public class PlayerStoreDatabaseSettings : IPlayerServiceDatabaseSettings
    {
        public string PlayersCollectionName { get; set; }
        public string AdminsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPlayerServiceDatabaseSettings
    {
        string PlayersCollectionName { get; set; }
        string AdminsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}