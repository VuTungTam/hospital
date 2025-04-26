namespace Hospital.SharedKernel.Infrastructure.Services.CIID
{
    public class CICodeUtility
    {
        public static bool ValidateCCCD(string cccd, int birthYear, int gender)
        {
            if (cccd.Length != 12)
            {
                return false;
            }

            if (!long.TryParse(cccd, out _))
            {
                return false;
            }

            int provinceCode = int.Parse(cccd[..3]);
            if (provinceCode < 1 || provinceCode > 96)
            {
                return false;
            }

            int genderCode = int.Parse(cccd[3].ToString());

            if ((birthYear < 2000 && (gender == 1 && genderCode != 0) || (gender == 0 && genderCode != 1)) ||
            (birthYear >= 2000 && (gender == 1 && genderCode != 2) || (gender == 0 && genderCode != 3)))
            {
                return false;
            }

            int cccdYear = int.Parse(cccd.Substring(4, 2));
            int year = birthYear % 100;
            if (cccdYear != year)
            {
                return false;
            }

            string randomNumbers = cccd.Substring(6);
            if (!long.TryParse(randomNumbers, out _))
            {
                return false;
            }

            return true;
        }
    }
}
