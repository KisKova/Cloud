using Contracts;
using Entities;

namespace EfcDataAccess.DAOs; 

public class RoomProfileDao : IRoomService {
    
    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public RoomProfileDao(SmartHomeSystemContext smartHomeSystemContext) {
        _smartHomeSystemContext = smartHomeSystemContext;
    }


    public Task<RoomProfile> CreateRoomProfile(RoomProfile roomProfile, long userId) {
        throw new NotImplementedException();
    }

    public Task<RoomProfile> DeleteRoomProfile(long roomProfileId) {
        throw new NotImplementedException();
    }

    public Task<RoomProfile> ModifyRoomProfile(RoomProfile updatedRoomProfile) {
        throw new NotImplementedException();
    }

    public Task<ICollection<RoomProfile>> GetUserSpecificRoomProfiles(long userId) {
        throw new NotImplementedException();
    }

    public Task<ICollection<RoomProfile>> GetDefaultRoomProfiles() {
        throw new NotImplementedException();
    }

    public Task<RoomProfile> RetrieveRoomProfileById(long roomProfileId) {
        throw new NotImplementedException();
    }

    public Task SetRoomProfileAsActive(long roomProfileId, long homeId) {
        throw new NotImplementedException();
    }

    public Task SetRoomProfileAsInactive(long roomProfileId) {
        throw new NotImplementedException();
    }

    public Task<RoomProfile> GetActiveRoomProfileForHome(long homeId) {
        throw new NotImplementedException();
    }
}