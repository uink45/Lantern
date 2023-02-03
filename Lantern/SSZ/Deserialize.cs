using System;

namespace Lantern.SSZ
{
	public static class Deserialize
	{
        public static TContainer DeserialiseSSZObject<TContainer>(byte[] bytes, SizePreset? preset)
        {
            var containerType = SszContainer.GetContainer<TContainer>(preset);
            (TContainer deserialized, var consumedBytes) = containerType.Deserialize(bytes);
            return deserialized;
        }
    }
}

