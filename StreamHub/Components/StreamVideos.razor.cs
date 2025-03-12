using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using StreamHub.Models.VideoMetadatas;
using StreamHub.Services.Foundations;

namespace StreamHub.Components
{
    public partial class StreamVideos : ComponentBase
    {
        [Inject]
        public IVideoMetadataService VideoMetadataService { get; set; }

        private List<VideoMetadata> videoMetadatas;
        private string selectedVideo;

        // Ma'lumotlarni yuklash
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Fallback uchun bosh ro'yxat
                videoMetadatas = (await VideoMetadataService.RetrieveAllVideoMetadatas())?.ToList()
                                 ?? new List<VideoMetadata>();

                StateHasChanged(); // UI yangilash
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading videos: {ex.Message}");
            }
        }

        // Virtualize komponenti uchun ma'lumotlarni yuklash
        private async ValueTask<ItemsProviderResult<VideoMetadata>> LoadVideoMetadatas(ItemsProviderRequest request)
        {
            // Null yoki bo'sh ro'yxatni boshqarish
            if (videoMetadatas == null || !videoMetadatas.Any())
            {
                return new ItemsProviderResult<VideoMetadata>(new List<VideoMetadata>(), 0);
            }

            var totalCount = videoMetadatas.Count;

            // StartIndex va Countni cheklash
            var startIndex = Math.Max(request.StartIndex, 0);
            var count = Math.Min(request.Count, totalCount - startIndex);

            var items = videoMetadatas
                .Skip(startIndex)
                .Take(count)
                .ToList();

            // Natijani qaytarish
            return new ItemsProviderResult<VideoMetadata>(items, totalCount);
        }

        // Videoni tanlash
        private void SelectVideo(string videoUrl)
        {
            selectedVideo = videoUrl;
        }

        // Videoni yopish
        private void CloseVideo()
        {
            selectedVideo = null;
        }
    }
}
