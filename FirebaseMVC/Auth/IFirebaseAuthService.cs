using System.Threading.Tasks;
using CanYouEvenWin.Auth.Models;

namespace CanYouEvenWin.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}