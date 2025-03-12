using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StreamHub.Models.VideoMetadatas;
using StreamHub.Services.Foundations;
using System;
using System.IO;
using System.Threading.Tasks;

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
            if (!ValidateInputs()) return;

            try
            {
                isUploading = true;
                Message = "Uploading...";
                StateHasChanged();

                // Upload files in parallel for faster processing
                var videoTask = SaveFileAsync(selectedVideoFile, "Videos");
                var thumbnailTask = SaveFileAsync(selectedThumbnailFile, "Thumbnails");

                await Task.WhenAll(videoTask, thumbnailTask);

                // Save metadata to database
                var videoMetadata = new VideoMetadata
                {
                    Id = Guid.NewGuid(),
                    Title = videoTitle,
                    Description = videoDescription,
                    VideoUrl = $"/Videos/{Path.GetFileName(videoTask.Result)}",
                    Thumbnail = $"/Thumbnails/{Path.GetFileName(thumbnailTask.Result)}",
                    CreatedDate = DateTimeOffset.UtcNow,
                    UpdatedDate = DateTimeOffset.UtcNow
                };

                await VideoMetadataService.AddVideoMetadataAsync(videoMetadata);

                Message = "<span style='color:green;'>Video uploaded successfully!</span>";
            }
            catch (Exception ex)
            {
                Message = $"<span style='color:red;'>Error: {ex.Message}</span>";
            }
            finally
            {
                isUploading = false;
                StateHasChanged();
            }
        }

        private bool ValidateInputs()
        {
            if (selectedVideoFile == null)
            {
                Message = "<span style='color:red;'>Please select a video file!</span>";
                return false;
            }

            if (selectedThumbnailFile == null)
            {
                Message = "<span style='color:red;'>Please select a thumbnail file!</span>";
                return false;
            }

            if (string.IsNullOrWhiteSpace(videoTitle) || string.IsNullOrWhiteSpace(videoDescription))
            {
                Message = "<span style='color:red;'>Please enter title and description!</span>";
                return false;
            }

            const long MaxVideoSize = 1073741824; // 1GB
            if (selectedVideoFile.Size > MaxVideoSize)
            {
                Message = "<span style='color:red;'>Video size must be less than 1GB!</span>";
                return false;
            }

            return true;
        }

        private async Task<string> SaveFileAsync(IBrowserFile file, string folderName)
        {
            string uploadsFolder = Path.Combine(Env.WebRootPath, folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(file.Size).CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
