using System.IO;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// XML script deserializer
    /// </summary>
    public interface IScriptDeserializer
    {
        /// <summary>
        /// Validates and deserializes the XML script
        /// provided in <paramref name="source"/>
        /// </summary>
        /// <remarks>
        /// Performs XML schema validation during deserialization
        /// </remarks>
        /// <param name="source">Opened stream with XML update script</param>
        /// <returns>Deserialized stream</returns>
        Updates GetDeserializedScript(Stream source);
    }
}