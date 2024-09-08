using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Ulids;

public static class UlidConverters
{
    public static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 26);
}

public class UlidValueConverter() : ValueConverter<Ulid, string>(e => e.ToString(),
    e => Ulid.Parse(e),
    UlidConverters.DefaultHints);