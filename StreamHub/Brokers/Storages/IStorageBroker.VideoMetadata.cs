using StreamHub.Models.VideoMetadatas;

namespace StreamHub.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata);
        ValueTask<IQueryable<VideoMetadata>> SelectAllVideoMetadatas();
        ValueTask<VideoMetadata> SelectVideoMetadataById(Guid videoMetadataId);
        ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata); 
    }
}
