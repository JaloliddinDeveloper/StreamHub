using StreamHub.Models.Foundations.VideoMetadatas;

namespace StreamHub.Services.Foundations.VideoMetadatas
{
    public interface IVideoMetadataService
    {
        ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata);
        ValueTask<IQueryable<VideoMetadata>> RetrieveAllVideoMetadatas();

    }
}