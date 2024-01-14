using Framework.AccountData;

namespace Framework.Server.CookieAndSession;

public class SessionManager
{
    private static Lazy<SessionManager> _sessionManager 
        = new Lazy<SessionManager>(() => new SessionManager());
    public static SessionManager Instance => _sessionManager.Value;
    private async Task<Session> CreateSession(
        int accountId, 
        string login, 
        bool unlimited, 
        TimeSpan? expiresIn, 
        DateTime createdTime)
    {
        var foundSessionTask = SessionData.Instance.GetSessionByAccountId(accountId);
        var guid = Guid.NewGuid();
        while (await SessionData.Instance.GetSessionById(guid) != null)
        {
            guid = Guid.NewGuid();
        }

        if (createdTime == default)
        {
            createdTime = DateTime.Now;
        }

        var session = new Session
        {
            Id = guid,
            AccountId = accountId,
            Login = login,
            Unlimited = unlimited,
            CreateDateTime = createdTime,
            Expires = createdTime + expiresIn
        };

        var foundSession = await foundSessionTask;
        if (foundSession == null)
        {
            if (unlimited || session.Expires > session.CreateDateTime)
            {
                await SessionData.Instance.Insert(session);
            }
        }
        else
        {
            if (unlimited || session.Expires > session.CreateDateTime)
            {
                await SessionData.Instance.Update(session);
            }
            else
            {
                await SessionData.Instance.Delete(foundSession);
            }
        }

        return session;
    }

    public async Task<Session> CreateSession(
        int accountId,
        string login,
        TimeSpan? expiresIn,
        DateTime createdTime = default) 
        => await CreateSession(accountId, login, false, expiresIn, createdTime);

    public async Task<Session> CreateUnlimitedSession(
        int accountId,
        string login,
        DateTime createdTime = default)
        => await CreateSession(accountId, login, true, null, createdTime);

    public async Task<Session?> GetSession(Guid id)
    {
        var session = await SessionData.Instance.GetSessionById(id);
        
        if (session?.Expires < DateTime.Now)
        {
            await SessionData.Instance.Delete(session);
            return null;
        }

        return session;
    }

    public async Task<bool> CheckSession(Guid id)
        => await GetSession(id) != null;
}
