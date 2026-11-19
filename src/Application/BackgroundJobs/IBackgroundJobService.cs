using System.Linq.Expressions;

namespace Application.BackgroundJobs;

public interface IBackgroundJobService
{
    string Enqueue<T>(Expression<Func<T, Task>> methodCall);
    bool Requeue(string jobId);
    string ContinueWith<T>(string parentJobId, Expression<Func<T, Task>> methodCall);
}
