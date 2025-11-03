using System.Threading;
using System.Threading.Tasks;

public interface IKitRepository
{
    Task<KitItem[]> FetchAsync(string url, CancellationToken ct = default);
}
