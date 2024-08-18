﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Ulids;

public class UlidToBytesConverter : ValueConverter<Ulid, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 16);

    public UlidToBytesConverter() : this(null)
    {
    }

    public UlidToBytesConverter(ConverterMappingHints? mappingHints = null)
        : base(
            convertToProviderExpression: x => x.ToByteArray(),
            convertFromProviderExpression: x => new Ulid(x),
            mappingHints: DefaultHints.With(mappingHints))
    {
    }
}

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 26);

    public UlidToStringConverter() : this(null)
    {
    }

    public UlidToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(
            convertToProviderExpression: x => x.ToString(),
            convertFromProviderExpression: x => Ulid.Parse(x),
            mappingHints: DefaultHints.With(mappingHints))
    {
    }
}