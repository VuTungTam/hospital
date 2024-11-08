using Hospital.SharedKernel.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.VisualBasic.FileIO;

namespace Hospital.SharedKernel.Libraries.Helpers
{
    public static class FileHelper
    {
        private static readonly List<string> ImageExtensions = new()
        {
            "apng",
            "avif",
            "gif",
            "jpg",
            "jpeg",
            "jfif",
            "pjpeg",
            "pjp",
            "png",
            "svg",
            "webp",
        };

        private static readonly List<string> VideoExtensions = new()
        {
            "m2v",
            "mpg",
            "mp2",
            "mpeg",
            "mpe",
            "mpv",
            "mp4",
            "m4p",
            "m4v",
            "mov",
        };

        public static string GetMimeType(string fileName)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var mimeType);
            return mimeType;
        }

        public static double ConvertBytesToKilobytes(long bytes)
        {
            return 1.0 * bytes / 1024;
        }

        public static double ConvertBytesToMegabytes(long bytes)
        {
            return ConvertBytesToKilobytes(bytes) / 1024;
        }

        public static double ConvertBytesToGigabytes(long bytes)
        {
            return ConvertBytesToMegabytes(bytes) / 1024;
        }

        public static long ConvertKilobytesToBytes(long kilobytes)
        {
            return kilobytes * 1024;
        }

        public static long ConvertMegabytesToBytes(long megabytes)
        {
            return megabytes * 1024 * 1024;
        }

        public static long ConvertGigabytesToBytes(long gigabytes)
        {
            return gigabytes * 1024 * 1024 * 1024;
        }

        public static string Format(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            var suffixIndex = 0;
            var size = bytes * 1.0;

            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                size /= 1024.0;
                suffixIndex++;
            }

            return $"{Math.Round(size, 2)} {suffixes[suffixIndex]}";
        }

        public static bool IsImage(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            return ImageExtensions.Find(e => $".{e}".Equals(extension, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public static bool IsImage(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }
            var extension = Path.GetExtension(file.FileName);
            return IsImage(extension);
        }

        public static bool IsImageByFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            var extension = Path.GetExtension(fileName);
            return IsImage(extension);
        }

        public static bool IsVideo(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            return VideoExtensions.Find(e => $".{e}".Equals(extension, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public static bool IsVideo(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }
            var extension = Path.GetExtension(file.FileName);
            return IsVideo(extension);
        }

        public static bool IsVideoByFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            var extension = Path.GetExtension(fileName);
            return IsVideo(extension);
        }

        public static FileType GetFileType(string extension)
        {
            if (IsImage(extension)) return FileType.Image;
            if (IsVideo(extension)) return FileType.Video;
            return FileType.Other;
        }

        public static FileType GetFileType(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            if (IsImage(extension))
                return FileType.Image;
            if (IsVideo(extension))
                return FileType.Video;
            return FileType.Other;
        }
    }
}
