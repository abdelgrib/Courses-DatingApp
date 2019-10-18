using DatingApp.API.Data;

namespace DatingApp.API.Business
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        public AuthService(IAuthRepository repository)
        {
            this._repository = repository;
        }
    }
}