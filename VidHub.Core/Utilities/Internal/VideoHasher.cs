using Blake3;
using System.Text;
using System.Text.Json;
using VidHub.Core.Settings;
using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Utilities.Internal
{
    internal class VideoHasher(Video video)
    {
        private readonly ILogger logger = VidHubContext.Logger;

        public string GenerateHash()
        {
            logger.LogTrace("GenerateHash entered for file={File}", video.FilePath);
            string baseHash = VidHubSettings.Instance.General.UseFileContentHash
                ? GenerateHash(File.OpenRead(video.FilePath))
                : GenerateHash(video.FilePath);

            string currentHash = baseHash;
            int salt = 0;
            while (HashCollides(Path.Combine(Path.GetTempPath(), "VidHub", "Cache", $"{currentHash}.json")))
            {
                salt++;
                logger.LogDebug("Hash collision detected for {Hash}, incrementing salt to {Salt}", currentHash, salt);
                currentHash = GenerateHash($"{baseHash}:{salt}");
            }

            logger.LogDebug("GenerateHash returning {Hash}", currentHash);
            return currentHash;
        }
        private string GenerateHash(Stream stream)
        {
            logger.LogTrace("GenerateHash(Stream) entered");
            Hasher hasher = Hasher.New();
            byte[] buffer = new byte[1024 * 1024 * 8];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hasher.Update(buffer.AsSpan(0, bytesRead));
            }
            Hash generatedHash = hasher.Finalize();
            string result = generatedHash.ToString().Replace("-", "").ToLowerInvariant();
            logger.LogDebug("GenerateHash(Stream) produced hash of length {Length}", result.Length);
            return result;
        }
        private string GenerateHash(string data)
        {
            logger.LogTrace("GenerateHash(string) entered");
            string result = Hasher.Hash(Encoding.UTF8.GetBytes(data)).ToString().Replace("-", "").ToLowerInvariant();
            logger.LogDebug("GenerateHash(string) produced hash of length {Length}", result.Length);
            return result;
        }

        private bool HashCollides(string cacheFilePath)
        {
            logger.LogTrace("HashCollides check for cacheFilePath={Path}", cacheFilePath);
            if (!File.Exists(cacheFilePath))
            {
                logger.LogDebug("No cache file at {Path}", cacheFilePath);
                return false;
            }

            Video? cache = JsonSerializer.Deserialize<Video>(File.ReadAllText(cacheFilePath));
            if (cache is not null && File.Exists(cache.FilePath))
            {
                FileInfo oldFile = new(cache.FilePath);
                FileInfo newFile = new(video.FilePath);

                if (oldFile.Length != newFile.Length)
                {
                    logger.LogDebug("Cache file size differs from current file size, collision=true");
                    return true;
                }

                int bytesRead = -1;
                while (bytesRead == 0)
                {
                    Stream oldFileStream = oldFile.OpenRead();
                    Stream newFileStream = newFile.OpenRead();
                    int bufferSize = 1024 * 1024 * 8;
                    byte[] oldBuffer = new byte[bufferSize];
                    byte[] newBuffer = new byte[bufferSize];

                    bytesRead = oldFileStream.Read(oldBuffer, 0, bufferSize) + newFileStream.Read(newBuffer, 0, bufferSize);

                    if (!oldBuffer.SequenceEqual(newBuffer))
                    {
                        logger.LogDebug("File contents differ during collision check, collision=true");
                        return true;
                    }
                }
            }

            logger.LogTrace("HashCollides returning false");
            return false;
        }
    }
}
