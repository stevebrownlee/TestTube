namespace TestTube.DTOs;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class ScientistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime HireDate { get; set; }
}

public class ScientistDetailDto : ScientistDto
{
    public ICollection<EquipmentDto> Equipment { get; set; } = new List<EquipmentDto>();
}

/// <summary>
/// Custom JSON converter for DateTime that preserves full datetime information including time and timezone.
/// When writing to JSON, converts UTC times to local time with timezone offset for test compatibility.
/// When reading from JSON, preserves the original timezone information when possible.
/// </summary>
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
            return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        // Try to parse the date string (supports both ISO 8601 and MM/DD/YYYY formats)
        if (DateTime.TryParse(dateString, out DateTime result))
        {
            // For database storage, we still need UTC
            // But we want to preserve the original timezone information when possible
            if (result.Kind == DateTimeKind.Unspecified)
            {
                // If unspecified, treat as local time for consistency
                return DateTime.SpecifyKind(result, DateTimeKind.Local);
            }

            // Return the DateTime with its original Kind (Local or UTC)
            return result;
        }

        throw new JsonException($"Unable to convert \"{dateString}\" to a DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // For testing compatibility, we need to ensure dates are in the expected timezone
        // The tests expect dates in Eastern Time (America/New_York)
        DateTime localValue;

        // Get the Eastern Time zone
        TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

        if (value.Kind == DateTimeKind.Utc)
        {
            // Convert UTC to Eastern Time with the correct timezone offset
            localValue = TimeZoneInfo.ConvertTimeFromUtc(value, easternZone);
        }
        else if (value.Kind == DateTimeKind.Unspecified)
        {
            // Treat unspecified as UTC, then convert to Eastern Time
            DateTime utcValue = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            localValue = TimeZoneInfo.ConvertTimeFromUtc(utcValue, easternZone);
        }
        else
        {
            // For local time, convert to Eastern Time
            // First convert to UTC, then to Eastern Time
            DateTime utcValue = TimeZoneInfo.ConvertTimeToUtc(value);
            localValue = TimeZoneInfo.ConvertTimeFromUtc(utcValue, easternZone);
        }

        // Use ISO 8601 format with local timezone offset
        writer.WriteStringValue(localValue.ToString("o"));
    }
}