using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IHomeService
    {
        Task<Home> AddNewHome(long userId, Home home);
        Task<Home> DeleteHome(long homeId);
        Task<Home> ModifyHome(Home home);
        Task<ICollection<Home>> RetrieveUserHomes(long userId);

        Task<Home> GetLastMeasurementAtHome();

        public Task<ICollection<Home>> RetrieveAllHomesFromSystem(); 
        Task<ICollection<LastMeasurement>> RetrieveHomesWithLastMeasurement(long userId);

        Task<long> RetrieveHomeIdByEui(string eui);
    }
}