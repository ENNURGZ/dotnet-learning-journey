using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Business.Tests;

internal static class UnitTestBusinessHelper
{
    public static IMapper CreateMapperProfile()
    {
        var myProfile = new AutomapperProfile();
        using var logger = new NullLoggerFactory();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile), logger);
        return new Mapper(configuration);
    }
}
