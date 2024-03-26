using WGMansion.Api.Models;
using WGMansion.MongoDB.Services;

namespace WGMansion.Api.ViewModels
{
    public interface IHealthcheckViewModel
    {
        HealthcheckResult Ping();
    }
    public class HealthcheckViewModel : IHealthcheckViewModel
    {
        private readonly IMongoService<Account> _mongoService;
        public HealthcheckViewModel(IMongoService<Account> mongoService) 
        {
            _mongoService = mongoService;
        }

        public HealthcheckResult Ping()
        {
            var mongoStatus = _mongoService.Ping();
            if (mongoStatus == 3) return HealthcheckResult.Healthy;
            if (mongoStatus > 0) return HealthcheckResult.Degraded;
            return HealthcheckResult.Down;
        }
    }
}
