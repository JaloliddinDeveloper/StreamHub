using Microsoft.AspNetCore.Components;
using StreamHub.Models.VideoMetadatas;
using StreamHub.Services.Foundations;
using System.Linq;
using System.Threading.Tasks;

namespace StreamHub.Components.Pages
{
    public partial class Delete
    {
        [Inject]
        public IVideoMetadataService VideoMetadataService { get; set; }

        private List<VideoMetadata> videoMetadatas = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadVideosAsync();
        }

        private async Task LoadVideosAsync()
        {
            videoMetadatas = (await VideoMetadataService.RetrieveAllVideoMetadatas()).ToList();
        }

        private async Task DeleteVideo(Guid id)
        {
            var videoToRemove = await VideoMetadataService.RemoveVideoMetadataSelectById(id);
            if (videoToRemove != null)
            {
                videoMetadatas.Remove(videoToRemove);
                StateHasChanged(); // UI yangilanadi
            }
        }
    }
}
