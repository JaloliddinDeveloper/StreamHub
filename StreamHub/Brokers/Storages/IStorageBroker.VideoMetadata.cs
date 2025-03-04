using StreamHub.Models.Foundations.VideoMetadatas;

namespace StreamHub.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata);
        ValueTask<IQueryable<VideoMetadata>> SelectAllVideoMetadatas();
    }
}
