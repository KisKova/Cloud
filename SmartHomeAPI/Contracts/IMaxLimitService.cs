using Entities;
using System.Threading.Tasks;

namespace Contracts;

    public interface IMaxLimitService
    {
        public  Task<ThresholdLimits> GetThresholdForRoomProfile(long roomProfileId);
        public  Task UpdateRoomProfileThreshold(ThresholdLimits thresholdLimits, long roomId);
        public  Task<ThresholdLimits> GetThresholdForActiveRoom(long homeId);
    }
