using UMG.DeveloperTest.Types;

namespace UMG.DeveloperTest.Services;

public interface IPaymentService
{
    MakePaymentResult MakePayment(MakePaymentRequest request);
}