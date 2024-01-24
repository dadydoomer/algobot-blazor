using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Algobot.Server.Authorization
{
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
