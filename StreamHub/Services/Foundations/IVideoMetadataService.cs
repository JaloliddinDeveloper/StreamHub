using StreamHub.Models.VideoMetadatas;

namespace StreamHub.Services.Foundations
{
    public interface IVideoMetadataService
    {
       ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
        ValueTask<IQueryable<VideoMetadata>> RetrieveAllVideoMetadatas();
        ValueTask<VideoMetadata>  RemoveVideoMetadataSelectById(Guid videoMetadataId);
    }
}