using System.ComponentModel;
using System.Reflection;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FOKE.Entity
{
    public static class GenericUtilities
    {
        public static string dateTimeFormat = "dd-MMM-yyyy HH:mm";
        public static string dateFormat = "dd-MMM-yyyy";
        public static string baseCurrency = "KWD";
        public static T Convert<T>(this object input)
        {
            if (input == null || input?.ToString() == "")
            {
                return default(T);
            }
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input?.ToString());
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
        public static string SetReportData(byte[] fileBytes, string extension)
        {
            try
            {
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }
                Guid obj = Guid.NewGuid();
                string binFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string tempPath = $"{binFolderPath}//TempReports//";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                string tempFileName = obj.ToString() + extension;
                System.IO.File.WriteAllBytes(tempPath + tempFileName, fileBytes);
                return tempFileName;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public static DateTime? ConvertToLocalDateTime(DateTime? inputDate)
        {
            DateTime? date = inputDate;
            try
            {
                if (inputDate != null && inputDate != DateTime.MinValue)
                {
                    TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
                    DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)inputDate, systemTimeZone);
                }
            }
            catch { }
            return date;
        }

        public static string ConvertAndFormatToLocalDateTime(DateTime? inputDate, string format)
        {
            string dateFormatted = "";
            try
            {
                if (inputDate != null && inputDate != DateTime.MinValue)
                {

                    TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

                    inputDate = DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Utc);
                    var localTime = inputDate.Value.ToLocalTime();

                    // DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)inputDate, systemTimeZone);
                    dateFormatted = localTime.ToString(format);
                }
            }
            catch { }
            return dateFormatted;
        }
        public static async Task<MemoryStream> GetReportData(string fileName)
        {
            try
            {

                Guid obj = Guid.NewGuid();
                string binFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string tempPath = $"{binFolderPath}//TempReports//";
                var memory = new MemoryStream();
                try
                {
                    using (var stream = new FileStream(tempPath + fileName, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    System.IO.File.Delete(tempPath + fileName);
                }
                catch (Exception e)
                {

                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static string FormatBytes(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;

            while (bytes >= 1024 && i < 4)
            {
                bytes /= 1024;
                i++;
            }

            return $"{bytes:n2} {units[i]}";
        }

        public static string ConvertAndFormatToKuwaitDateTime(DateTime? inputDate, string format)
        {
            string dateFormatted = "";
            try
            {
                if (inputDate != null && inputDate != DateTime.MinValue)
                {

                    if (inputDate != null && inputDate != DateTime.MinValue)
                    {
                        DateTime adjustedDate = inputDate.Value.AddHours(3);
                        dateFormatted = adjustedDate.ToString(format);
                    }
                }
            }
            catch { }
            return dateFormatted;
        }

        public static decimal ExtractNumericValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            var numericPart = new string(input
                .Where(c => char.IsDigit(c) || c == '.')
                .ToArray());

            return decimal.TryParse(numericPart, out var result) ? result : 0;
        }
        public static bool IsAllowedExtension(string extension)
        {
            string[] allowedExtensions = { ".jpeg", ".jpg", ".png", ".doc", ".docx", ".pdf", ".txt", ".gif", ".xls", ".xlsx" };
            return allowedExtensions.Contains(extension.ToLower());
        }
        public static DateTime? ExtractDobFromCivilId(string civilId)
        {
            if (string.IsNullOrWhiteSpace(civilId) || civilId.Length != 12)
                return null;

            try
            {
                var centuryPrefix = civilId.StartsWith("2") ? "19" : civilId.StartsWith("3") ? "20" : null;
                if (centuryPrefix == null) return null;

                var year = centuryPrefix + civilId.Substring(1, 2);
                var month = civilId.Substring(3, 2);
                var day = civilId.Substring(5, 2);

                var dobString = $"{year}-{month}-{day}";
                if (DateTime.TryParseExact(dobString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dob))
                    return dob;
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
