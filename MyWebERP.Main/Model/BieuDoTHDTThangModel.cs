namespace MyWebERP.Model
{
    public class BieuDoTHDTThangResultModel: APIResultDataModel
    {
        public decimal total_amount { get; set; }
    }
    public class BieuDoTHDTThangModel
    {
        public string m_cap { get; set; }
        public decimal amount { get; set; }
        //public decimal amountInBillion => Math.Round(amount / 1_000_000_000M, 2);

        //public decimal amountInMillion => Math.Round(amount / 1_000_000M, 2);

        //public string FormattedAmount => FormatMoney(amount);

        //private static string FormatMoney(decimal value)
        //{
        //    if (value >= 1_000_000_000)
        //        return $"{Math.Round(value / 1_000_000_000M, 1)} tỷ";
        //    else if (value >= 1_000_000)
        //        return $"{Math.Round(value / 1_000_000M, 1)} triệu";
        //    else
        //        return $"{value:N0}";
        //}
    }
}
