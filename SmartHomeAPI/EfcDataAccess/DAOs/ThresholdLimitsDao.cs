using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

public class ThresholdLimitsDao : IMaxLimitService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public ThresholdLimitsDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }


    public async Task<ThresholdLimits> FetchThresholdForSpecificRoomProfile(long roomProfileId) {
        RoomProfile profile;
        try
        {
            profile = await _smartHomeSystemContext.RoomProfiles!.Include(p => p.Limits)
                    .FirstAsync(p => p.RoomProfileId == roomProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }

        return profile.Limits;
    }

    public async Task UpdateThresholdForRoomProfile(ThresholdLimits updatedThresholdLimits, long roomProfileId) {
        RoomProfile profile;
        try
        {
            profile = await _smartHomeSystemContext.RoomProfiles!.Include(p => p.Limits)
                    .FirstAsync(p => p.RoomProfileId == roomProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }

        profile.Limits = updatedThresholdLimits;
        _smartHomeSystemContext.RoomProfiles!.Update(profile);
        await _smartHomeSystemContext.SaveChangesAsync();
    }

    public Task<ThresholdLimits> RetrieveThresholdForCurrentRoom(long homeId) {
        throw new NotImplementedException();
    }
    
    
}