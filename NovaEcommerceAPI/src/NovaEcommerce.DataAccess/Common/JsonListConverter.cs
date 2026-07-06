using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NovaEcommerce.DataAccess.Common;

// Shared List<string> <-> JSON column converter, used for the "photos" arrays on
// Review and ReturnRequest. A ValueComparer is required so EF's change tracker can
// correctly detect mutations to the list (default reference equality won't catch them).
public static class JsonListConverter
{
    public static ValueConverter<List<string>, string> StringList { get; } = new(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

    public static ValueComparer<List<string>> StringListComparer { get; } = new(
        (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
        v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
        v => v.ToList());
}
