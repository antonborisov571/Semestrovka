using Framework.Server.Helpers;
using ORM.ORMPostgres;
using System.Reflection;
using System.Security.Principal;

namespace Framework.AccountData;

public class AccountsData
{
    public static async Task<Account?> GetAccountById(int id)
        => (await MyORM.Instance.Select<Account>())
            .FirstOrDefault(x => x.Id == id);

    public static async Task<Account?> GetAccountByLoginAndPassword(string login, string password)
    {
        var accountCandidate = await GetAccountByLogin(login);

        if (accountCandidate is null)
            return accountCandidate;

        return PasswordHashingManager.GetSHA256(password + accountCandidate.Salt) == accountCandidate.Password ?
            accountCandidate :
            null;
    }

    public static async Task<Account?> GetAccountByLogin(string login)
        => (await MyORM.Instance.Select<Account>())
            .FirstOrDefault(x => x.Login == login);

    public static async Task<TEntity?> GetAccountByLogin<TEntity>(TEntity obj, string login) where TEntity : Account
        => (await MyORM.Instance.Select<TEntity>())
            .FirstOrDefault(x => x.Login == login);

    public static async Task<TEntity?> GetAccountById<TEntity>(TEntity obj, int id) where TEntity : Account
        => (await MyORM.Instance.Select<TEntity>())
            .FirstOrDefault(x => x.Id == id);

    public static async Task<TEntity?> GetAccountByLoginAndPassword<TEntity>(TEntity obj, string login, string password)
        where TEntity : Account
    {
        var accountCandidate = await GetAccountByLogin(obj, login);

        if (accountCandidate is null)
            return accountCandidate;

        return PasswordHashingManager.GetSHA256(password + accountCandidate.Salt) == accountCandidate.Password ?
            accountCandidate :
            null;
    }
}
