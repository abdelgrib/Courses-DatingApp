using DatingApp.API.Data;

namespace DatingApp.API.Business
{
    public class DatingService : IDatingService
    {
        private readonly IDatingRepository _repository;
        public DatingService(IDatingRepository repository)
        {
            this._repository = repository;
        }
    }
}