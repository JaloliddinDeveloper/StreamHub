using Microsoft.AspNetCore.Components;
using StreamHub.Models.VideoMetadatas;
using StreamHub.Services.Foundations;

namespace StreamHub.Components
{
    public partial class StreamVideos : ComponentBase
    {
        [Inject]
        public IVideoMetadataService VideoMetadataService { get; set; }

        private IQueryable<VideoMetadata> videoMetadatas;
        private string selectedVideo;
        private int clickCount = 0;
        private DateTime lastClickTime = DateTime.MinValue;
        protected override async Task OnInitializedAsync()
        {
            videoMetadatas = await VideoMetadataService.RetrieveAllVideoMetadatas();
        }

        private void SelectVideo(string videoUrl)
        {
            selectedVideo = videoUrl;
            clickCount = 0; // Reset click count
        }

        private void HandleMouseDown()
        {
            var now = DateTime.Now;
            if ((now - lastClickTime).TotalMilliseconds < 500) // Check if it's a double-click
            {
                CloseVideo();
            }
            lastClickTime = now;
        }

        private void CloseVideo()
        {
            selectedVideo = null;
        }
    }
}
