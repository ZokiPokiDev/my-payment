using UMG.DeveloperTest.Requests;
using UMG.DeveloperTest.Results;

namespace UMG.DeveloperTest.Services;

public interface IPaymentService
{
    MakePaymentResult MakePayment(MakePaymentRequest request);
}