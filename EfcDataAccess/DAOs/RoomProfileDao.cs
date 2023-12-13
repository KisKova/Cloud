using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

public class RoomProfileDao : IRoomService {
    
    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public RoomProfileDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }


    public async Task<RoomProfile> CreateRoomProfile(RoomProfile roomProfile, long userId) {
        try
        {
            await _smartHomeSystemContext.ThresholdsLimits!.AddAsync(roomProfile.Threshold);
            await _smartHomeSystemContext.RoomProfiles!.AddAsync(roomProfile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something wrong happened when adding to the database.");
        }

        User user;

        try
        {
            user = await _smartHomeSystemContext.Users!.Include(u => u.RoomProfiles).FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }

        await _smartHomeSystemContext.SaveChangesAsync();

        try
        {
            user.RoomProfiles!.Add(roomProfile);
            _smartHomeSystemContext.Users!.Update(user);

            await _smartHomeSystemContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something wrong when adding to the database.");
        }

        return roomProfile;
    }

    public async Task<RoomProfile> DeleteRoomProfile(long roomProfileId) {
        RoomProfile roomProfile;
        try
        {
            roomProfile = await _smartHomeSystemContext.RoomProfiles!.FirstAsync(p => p.Id == roomProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }

        RoomProfile profile = roomProfile;
        _smartHomeSystemContext.RoomProfiles!.Remove(roomProfile);
        await _smartHomeSystemContext.SaveChangesAsync();
        return profile;
    }

    public async Task<RoomProfile> ModifyRoomProfile(RoomProfile updatedRoomProfile) {
        try
        {
            _smartHomeSystemContext.RoomProfiles!.Update(updatedRoomProfile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("A problem occured while updating the room profile.");
        }

        await _smartHomeSystemContext.SaveChangesAsync();
        return updatedRoomProfile;
    }

    public async Task<ICollection<RoomProfile>> GetUserSpecificRoomProfiles(long userId) {

        User u;
        try
        {
            u = await _smartHomeSystemContext.Users!
                    .Include(u => u.RoomProfiles)
                    .FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }

        return u.RoomProfiles!;
    }

    public async Task<ICollection<RoomProfile>> GetDefaultRoomProfiles() {

        ICollection<RoomProfile> defaultRoomProfiles = new List<RoomProfile>();
        try
        {
            defaultRoomProfiles = await _smartHomeSystemContext.RoomProfiles!
                    .Include(p => p.Threshold)
                    .Where(p => p.IsDefault == true)
                    .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("No default room profiles found.");
        }

        return defaultRoomProfiles;
    }

    public async Task<RoomProfile> RetrieveRoomProfileById(long roomProfileId) {
        RoomProfile roomProfile;
        try
        {
            roomProfile = await _smartHomeSystemContext.RoomProfiles!.FirstAsync(p => p.Id == roomProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }

        return roomProfile;
    }

    public async Task SetRoomProfileAsActive(long roomProfileId, long homeId) {
        RoomProfile roomProfile;
        try
        {
            roomProfile = await _smartHomeSystemContext.RoomProfiles!.FirstAsync(p => p.Id == roomProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Room profile not found.");
        }

        Home home;
        try
        {
            home = await _smartHomeSystemContext.Homes!.FirstAsync(h => h.HomeId == homeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Homes not found.");
        }

        home.CurrentRoomProfile = roomProfile;
        _smartHomeSystemContext.Homes!.Update(home);
        await _smartHomeSystemContext.SaveChangesAsync();
    }

    public async Task SetRoomProfileAsInactive(long homeProfileId) {

        Home home;
        try
        {
            home = await _smartHomeSystemContext.Homes!.Include(h => h.CurrentRoomProfile)
                    .FirstAsync(h => h.HomeId == homeProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Home not found.");
        }

        home.CurrentRoomProfile = null;
        _smartHomeSystemContext.Homes!.Update(home);
        await _smartHomeSystemContext.SaveChangesAsync();
    }

    public async Task<RoomProfile> GetActiveRoomProfileForHome(long homeId) {
        Home home;
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

        if (home.CurrentRoomProfile != null)
        {
            return home.CurrentRoomProfile!;
        }

        throw new Exception($"There is no active room profile on this home: {homeId}");
    }
}