using StreamHub.Brokers.Storages;
using StreamHub.Models.Foundations.VideoMetadatas;

namespace StreamHub.Services.Foundations.VideoMetadatas
{
    public class VideoMetadataService: IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;

        public VideoMetadataService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata)
        {
            videoMetadata.Id = Guid.NewGuid();
            videoMetadata.CreatedDate = DateTimeOffset.UtcNow;
            videoMetadata.UpdatedDate = DateTimeOffset.UtcNow;

           return await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);
        }

        public async ValueTask<IQueryable<VideoMetadata>> RetrieveAllVideoMetadatas()
        {
           return await this.storageBroker.SelectAllVideoMetadatas();
        }
    }
}
