using Microsoft.AspNetCore.Components.Forms;
using StreamHub.Models.VideoMetadatas;

namespace StreamHub.Components
{
    public partial class UploadVideo
    {
        private IBrowserFile? selectedVideoFile;
        private IBrowserFile? selectedThumbnailFile;
        private string videoTitle = "";
        private string videoDescription = "";
        private string Message = "";
        private bool isUploading = false;

        private void HandleVideoFileSelected(InputFileChangeEventArgs e)
        {
            selectedVideoFile = e.File;
        }

        private void HandleThumbnailFileSelected(InputFileChangeEventArgs e)
        {
            selectedThumbnailFile = e.File;
        }

        private async Task SaveVideoToWwwRoot()
        {
            if (selectedVideoFile == null)
            {
                Message = "Iltimos, video faylni tanlang!";
                return;
            }
            if (selectedThumbnailFile == null)
            {
                Message = "Iltimos, thumbnail faylni tanlang!";
                return;
            }
            if (string.IsNullOrWhiteSpace(videoTitle) || string.IsNullOrWhiteSpace(videoDescription))
            {
                Message = "Iltimos, Title va Description ni kiriting!";
                return;
            }

            const long MaxVideoSize = 1073741824;
            if (selectedVideoFile.Size > MaxVideoSize)
            {
                Message = "Xatolik: Video hajmi 1GB dan oshmasligi kerak!";
                return;
            }

            try
            {
                isUploading = true;
                Message = "Biroz kuting, video yuklanmoqda...";
                StateHasChanged();

                // wwwroot ichidagi to‘g‘ridan-to‘g‘ri yo‘nalish
                string uploadsFolder = Path.Combine(this.Env.WebRootPath, "Videos");
                string thumbnailsFolder = Path.Combine(this.Env.WebRootPath, "Thumbnails");

                // Papkalarni yaratish
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                if (!Directory.Exists(thumbnailsFolder))
                {
                    Directory.CreateDirectory(thumbnailsFolder);
                }

                string uniqueVideoFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedVideoFile.Name)}";
                string videoFilePath = Path.Combine(uploadsFolder, uniqueVideoFileName);

                using (var stream = new FileStream(videoFilePath, FileMode.Create))
                {
                    await selectedVideoFile.OpenReadStream(MaxVideoSize).CopyToAsync(stream);
                }

                string uniqueThumbnailFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedThumbnailFile.Name)}";
                string thumbnailFilePath = Path.Combine(thumbnailsFolder, uniqueThumbnailFileName);

                using (var thumbStream = new FileStream(thumbnailFilePath, FileMode.Create))
                {
                    await selectedThumbnailFile.OpenReadStream(10485760).CopyToAsync(thumbStream);
                }

                VideoMetadata videoMetadata = new VideoMetadata
                {
                    Id = Guid.NewGuid(),
                    Title = videoTitle,
                    Description = videoDescription,
                    VideoUrl = $"/Videos/{uniqueVideoFileName}",
                    Thumbnail = $"/Thumbnails/{uniqueThumbnailFileName}",
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow
                };

                await VideoMetadataService.AddVideoMetadataAsync(videoMetadata);
                Message = "Video yuklandi!";
            }
            catch (Exception ex)
            {
                Message = "Xatolik: " + ex.Message;
            }
            finally
            {
                isUploading = false;
                StateHasChanged();
            }
        }

    }
}
