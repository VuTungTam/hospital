namespace Hospital.Application.Models.Dashboards.Customers
{
    public class CustomerTrend
    {
        public int Value { get; set; }

        public int SamePeriodLastYearValue { get; set; }

        public string Explain
        {
            get
            {
                if (Value > SamePeriodLastYearValue)
                {
                    return $"Tăng {Percent:N0}% so với cùng kỳ năm trước";
                }
                else if (Value < SamePeriodLastYearValue)
                {
                    return $"Giảm {Percent:N0}% so với cùng kỳ năm trước";
                }

                return "Giữ nguyên so với cùng kỳ năm trước";
            }
        }

        public string CompareExplain { get; set; }
        public string CompareExplainEn { get; set; }

        public double Percent => ((Value * 1.0 - SamePeriodLastYearValue) / (SamePeriodLastYearValue == 0 ? 1 : SamePeriodLastYearValue)) * 100;

        public string PercentText => Percent.ToString("N0") + "%";
    }
}
