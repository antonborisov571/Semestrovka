using Server.Model;

namespace Server.Extensions;

public static class AccountExtensions
{
    public static string GetAuthorName(this IEnumerable<Account> accounts, int? id)
        => accounts.FirstOrDefault(x => x.Id == id)?.Login ?? "Аноним";
}
