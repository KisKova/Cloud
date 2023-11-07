using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess.DAOs; 

//inject interface from Contracts.
public class UserDao : IUserService {

    private readonly SmartHomeSystemContext _smartHomeSystemContext;

    public UserDao(SmartHomeSystemContext smartHomeSystemContext) {
        this._smartHomeSystemContext = smartHomeSystemContext;
    }


    public async Task<User> GetUserByUsername(string username) {

        try
        {
            return await _smartHomeSystemContext.Users!.FirstAsync(u => u.Username == username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that username.");
        }
    }

    public async Task<User> GetUserById(long id) {

        try
        {
            return await _smartHomeSystemContext.Users!.FirstAsync(u => u.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that id.");
        }
    }
    
    //public async Task<User> GetHomeUser(long Id)
    //return Home for the set user Id.

    public async Task<User> GetUserByEmail(string email) {

        try
        {
            return await _smartHomeSystemContext.Users!.FirstAsync(u => u.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that email.");
        }
    } 

    public async Task<User> RegisterUser(User user) {

        ICollection<User> users;
        try
        {
            user.Token = null;
            await _smartHomeSystemContext.Users!.AddAsync(user);
        }
        catch (Exception sqlException)
        {
            Console.WriteLine(sqlException);
            throw new ("Something went wrong while adding the user. Please try again. ");
        }

        try
        {
            await _smartHomeSystemContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User with the same email already registered.");
        }

        return await _smartHomeSystemContext.Users!.FirstAsync(u => u.Email == user.Email);
    }

    public async Task<User> RemoveUser(long userId) {
        User user;
        try
        {
            user = await _smartHomeSystemContext.Users!.FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that id.");
        }

        User u = user;
        _smartHomeSystemContext.Users!.Remove(user);
        await _smartHomeSystemContext.SaveChangesAsync();
        return u;
    }

    public async Task<User> UpdateUser(User user) {

        try
        {
            _smartHomeSystemContext.Users!.Update(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something went wrong while updating user.");
        }

        await _smartHomeSystemContext.SaveChangesAsync();

        return await _smartHomeSystemContext.Users!.FirstAsync(u => u.Email == user.Email);
    }

    //Add encryption to password parameter
    public async Task<User> LogUserIn(string email, string password) {

        User user;
        try
        {
            user = await _smartHomeSystemContext.Users!.FirstAsync(u => u.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User with the provided email not found.");
        }

        if (user.Password == password)
        {
            return user;
        }

        throw new Exception("The credentials provided are incorrect.");
    }

    public async Task SetTokenForUser(long uId, string token) {

        User user;

        try
        {
            user = await _smartHomeSystemContext.Users!.FirstAsync(u => u.Id == uId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        user.Token = token;
        _smartHomeSystemContext.Users!.Update(user);
        await _smartHomeSystemContext.SaveChangesAsync();
    }

    public async Task RemoveTokenFromUser(long uId) {

        User user;
        try
        {
            user = await _smartHomeSystemContext.Users!.FirstAsync(u => u.Id == uId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }

        user.Token = null;
        _smartHomeSystemContext.Users!.Update(user);
        await _smartHomeSystemContext.SaveChangesAsync();
    }
}