using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Domain.constant;
public static class JsonSerializerSetting
{
    public static JsonSerializerOptions JsonSerializerOptions =>
        new()
        {
            PropertyNameCaseInsensitive = false,
            MaxDepth = 10,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic),
        };

    public static JsonSerializerOptions JsonDeserializerOptions =>
        new()
        {
            PropertyNameCaseInsensitive = true,
            MaxDepth = 10,
        };
}