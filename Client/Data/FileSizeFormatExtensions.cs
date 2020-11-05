using System;

namespace blazoract.Client.Data
{
    public static class FileSizeFormatExtensions
    {
        public static string ToDisplayString(this long fileSizeBytes)
        {
            int scale = 1024;
            string[] units = new string[] { "gigabytes", "megabytes", "kilobytes", "bytes" };
            long max = (long)Math.Pow(scale, units.Length - 1);

            foreach (var unit in units)
            {
                if (fileSizeBytes > max)
                    return $"{decimal.Divide(fileSizeBytes, max):##.##} {unit}";

                max /= scale;
            }
            return "0 bytes";
        }
    }
}
