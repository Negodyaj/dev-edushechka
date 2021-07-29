namespace DevEdu.API.Models.OutputModels.Payment
{
    public class PaymentOutputModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public decimal Sum { get; set; }
        public UserInfoShortOutputModel UserId { get; set; }
        public bool IsPaid { get; set; }
    }
}
