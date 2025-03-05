using StreamHub.Brokers.Storages;
using StreamHub.Models.VideoMetadatas;

namespace StreamHub.Services.Foundations
{
    public class VideoMetadataService : IVideoMetadataService
    {
        private readonly IStorageBroker storageBroker;

        public VideoMetadataService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<VideoMetadata> AddVideoMetadataAsync(VideoMetadata videoMetadata) =>
            await this.storageBroker.InsertVideoMetadataAsync(videoMetadata);

        public async ValueTask<IQueryable<VideoMetadata>> RetrieveAllVideoMetadatas() =>
            await this.storageBroker.SelectAllVideoMetadatas();

        public async ValueTask<VideoMetadata> RemoveVideoMetadataSelectById(Guid videoMetadataId)
        {
            VideoMetadata videoMetadata =
                await this.storageBroker.SelectVideoMetadataById(videoMetadataId);

            return await this.storageBroker.DeleteVideoMetadataAsync(videoMetadata);
        }
    }
}

