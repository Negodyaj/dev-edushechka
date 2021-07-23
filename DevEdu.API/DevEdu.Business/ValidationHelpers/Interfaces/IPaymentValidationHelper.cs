namespace DevEdu.Business.ValidationHelpers
{
    public interface IPaymentValidationHelper
    {
        void CheckPaymentExistence(int paymentId);
    }
}