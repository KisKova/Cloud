using Entities; // Import the necessary entity classes
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMaxLimitService
    {
        Task<ThresholdLimits> FetchThresholdForSpecificRoomProfile(long roomProfileId);
        Task UpdateThresholdForRoomProfile(ThresholdLimits updatedThresholdLimits, long roomProfileId);
        Task<ThresholdLimits?> RetrieveThresholdForCurrentRoom(long homeId);
    }
}