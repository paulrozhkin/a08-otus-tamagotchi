namespace Resources.API.Config
{
    public class ResourcesDatabaseOptions
    {
        public const string ResourcesDatabase = nameof(ResourcesDatabase);

        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ResourcesCollectionName { get; set; } = null!;
    }
}
