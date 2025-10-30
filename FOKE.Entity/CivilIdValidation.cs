using System.Text.RegularExpressions;

namespace FOKE.Entity
{
    public class CivilIdValidation
    {
        public static bool IsValidKuwaitCivilID(string civilId)
        {
            if (string.IsNullOrEmpty(civilId) || !Regex.IsMatch(civilId, @"^[23]\d{11}$"))
                return false;

            int[] weights = { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;

            for (int i = 0; i < 11; i++)
            {
                sum += (civilId[i] - '0') * weights[i];
            }

            int checksum = 11 - (sum % 11);
            if (checksum == 11) checksum = 0;

            // If checksum is 10, it's invalid
            if (checksum == 10) return false;

            return (civilId[11] - '0') == checksum;
        }
    }
}
