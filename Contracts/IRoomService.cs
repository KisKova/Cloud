using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Contracts;

public interface IRoomService
{

    public Task<RoomProfile> CreateRoomProfile(RoomProfile roomProfile, long userId);
    public Task<RoomProfile> DeleteRoomProfile(long roomProfileId);
    public Task<RoomProfile> ModifyRoomProfile(RoomProfile updatedRoomProfile);
    public Task<ICollection<RoomProfile>> GetUserSpecificRoomProfiles(long userId);
    public Task<ICollection<RoomProfile>> GetDefaultRoomProfiles();
    public Task<RoomProfile> RetrieveRoomProfileById(long roomProfileId);
    public Task SetRoomProfileAsActive(long roomProfileId, long homeId);
    public Task SetRoomProfileAsInactive(long roomProfileId);
    Task<RoomProfile> GetActiveRoomProfileForHome(long homeId);



}