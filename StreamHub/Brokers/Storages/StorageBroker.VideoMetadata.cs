using Microsoft.EntityFrameworkCore;
using StreamHub.Models.VideoMetadatas;

namespace StreamHub.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<VideoMetadata> VideoMetadatas { get; set; }

        public async ValueTask<VideoMetadata> InsertVideoMetadataAsync(VideoMetadata videoMetadata) =>
            await InsertAsync(videoMetadata);

        public ValueTask<IQueryable<VideoMetadata>> SelectAllVideoMetadatas() =>
            SelectAllAsync<VideoMetadata>();

        public async ValueTask<VideoMetadata> SelectVideoMetadataById(Guid videoMetadataId) =>
            await SelectAsync<VideoMetadata>(videoMetadataId);

        public async ValueTask<VideoMetadata> DeleteVideoMetadataAsync(VideoMetadata videoMetadata) =>
            await DeleteAsync(videoMetadata);
    }
}
