namespace eBoardAPI.Models.FundIncome
{
    public class CreateFundIncomeDto
    {
        public string Title { get; set; } = string.Empty;
        public int AmountPerStudent { get; set; }
        public DateOnly EndDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public string ValidateData()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return "Tiêu đề không được để trống.";
            }
            if (AmountPerStudent <= 0)
            {
                return "Số tiền mỗi học sinh phải lớn hơn 0.";
            }
            if (EndDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return "Ngày kết thúc không được trước ngày hiện tại.";
            }
            return string.Empty;
        }
    }
}
