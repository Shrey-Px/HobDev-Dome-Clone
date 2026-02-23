namespace Dome.Shared.Synced
{
    public partial class LearningContent : IRealmObject
    {
        private LearningContent() { }

        public LearningContent(
            ObjectId id,
            string type,
            string title,
            string description,
            string content,
            DateTimeOffset created,
            string publishedBy,
            string gameCategory
        )
        {
            Id = id;
            Type = type;
            Title = title;
            Description = description;
            Content = content;
            Created = created;
            PublishedBy = publishedBy;
            GameCategory = gameCategory;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("type")]
        public string Type { get; set; }

        [MapTo("title")]
        public string Title { get; set; }

        [MapTo("description")]
        public string Description { get; set; }

        [MapTo("content")]
        public string Content { get; set; }

        [Indexed]
        [MapTo("gameCategory")]
        public string GameCategory { get; set; }

        [MapTo("created")]
        public DateTimeOffset Created { get; set; }

        [MapTo("publishedBy")]
        public string PublishedBy { get; set; }
    }
}
