using Blake3;
using System.Text;
using System.Text.Json;
using VidHub.Core.Settings;

namespace VidHub.Core.Utilities.Internal
{
    internal class VideoHasher(Video video)
    {
        public string GenerateHash()
        {
            string baseHash = VidHubSettings.Instance.General.UseFileContentHash
                ? GenerateHash(File.OpenRead(video.FilePath))
                : GenerateHash(video.FilePath);

            string currentHash = baseHash;
            int salt = 0;
            while (HashCollides(Path.Combine(Path.GetTempPath(), "VidHub", "Cache", $"{currentHash}.json")))
            {
                salt++;
                currentHash = GenerateHash($"{baseHash}:{salt}");
            }

            return currentHash;
        }
        private string GenerateHash(Stream stream)
        {
            Hasher hasher = Hasher.New();
            byte[] buffer = new byte[1024 * 1024 * 8];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hasher.Update(buffer.AsSpan(0, bytesRead));
            }
            Hash generatedHash = hasher.Finalize();
            return generatedHash.ToString().Replace("-", "").ToLowerInvariant();
        }
        private string GenerateHash(string data)
        {
            return Hasher.Hash(Encoding.UTF8.GetBytes(data)).ToString().Replace("-", "").ToLowerInvariant();
        }

        private bool HashCollides(string cacheFilePath)
        {
            if (!File.Exists(cacheFilePath))
            {
                return false;
            }

            Video? cache = JsonSerializer.Deserialize<Video>(File.ReadAllText(cacheFilePath));
            if (cache is not null && File.Exists(cache.FilePath))
            {
                FileInfo oldFile = new(cache.FilePath);
                FileInfo newFile = new(video.FilePath);

                if (oldFile.Length != newFile.Length)
                {
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
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
