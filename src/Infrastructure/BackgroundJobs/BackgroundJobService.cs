using System.Linq.Expressions;
using Application.BackgroundJobs;
using Hangfire;

namespace Infrastructure.BackgroundJobs;

public class BackgroundJobService(IBackgroundJobClient backgroundJobClient) : IBackgroundJobService
{
    public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        => backgroundJobClient.Enqueue(methodCall);

    public bool Requeue(string jobId)
        => backgroundJobClient.Requeue(jobId);

    public string ContinueWith<T>(string parentJobId, Expression<Func<T, Task>> methodCall)
        => backgroundJobClient.ContinueJobWith(parentJobId, methodCall);
}
