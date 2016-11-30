using System.Threading.Tasks;

namespace QuickShopper.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
