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

    public async Task<ThresholdLimits?> RetrieveThresholdForCurrentRoom(long homeId) {
        long pId;
        Home home;
        Console.WriteLine("Func is started!!!!");
        try
        {
            home = await _smartHomeSystemContext.Homes!.Include(h => h.CurrentRoomProfile)
                    .FirstAsync(h => h.HomeId == homeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home not found.");
        }

        Console.WriteLine("This will be the problem... OR NOT?!");
        
        if (home.CurrentRoomProfile == null)
        {
            Console.WriteLine("There is no roomProfileId set!");
            return null;
        }

        pId = home.CurrentRoomProfile!.RoomProfileId;
        RoomProfile profile;
        try
        {
            profile = await _smartHomeSystemContext.RoomProfiles!.Include(p => p.Limits)
                .FirstAsync(p => p.RoomProfileId == pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }
            
        Console.WriteLine("The threshold limits exist.");
        return profile.Limits;
    }

}