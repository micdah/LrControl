using System;
using LrControl.Devices;
using Newtonsoft.Json;
using RtMidi.Core.Enums;

namespace LrControl.Configurations.Models
{
    public class ControllerIdModelConverter : JsonConverter<ControllerIdModel>
    {
        public override void WriteJson(JsonWriter writer, ControllerIdModel value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.Channel}-{value.MessageType}-{value.Parameter}");
        }

        public override ControllerIdModel ReadJson(JsonReader reader, Type objectType, ControllerIdModel existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new JsonSerializationException(
                    $"Cannot deserialize ControllerId from token of type {reader.TokenType}");

            var value = reader.Value.ToString();
            var parts = value.Split('-');
            if (parts.Length != 3)
                throw new JsonSerializationException(
                    $"Incorrect format of '{value}', must contain exactly three parts separated by '-'");

            if (!Enum.TryParse<Channel>(parts[0], out var channel))
                throw new JsonSerializationException("Could not parse Channel");
            
            if (!Enum.TryParse<MessageType>(parts[1], out var messageType))
                throw new JsonSerializationException("Could not parse MessageType");

            if (!int.TryParse(parts[2], out var parameter))
                throw new JsonSerializationException("Could not parse Parameter");

            return new ControllerIdModel
            {
                Channel = channel,
                MessageType = messageType,
                Parameter = parameter
            };
        }
    }
}