using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using TaskSubscription.RestAPI.Common;

NLog.ILogger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    var startUp = new StartUp(builder);
    //-----------------------------------------------
    startUp
        .AddServicesAndBuild()
        .AddPipeLines()
        .Run();

}
catch (Exception exc)
{
    logger.Error(exc, "Application Error in error");
    throw;
}
