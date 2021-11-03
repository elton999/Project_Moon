using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TWrite = ldtk.LdtkJson;

namespace MonoGame.ContentPipeline.UmbrellaToolsKit
{
    [ContentTypeWriter]
    public class LdtkWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(Newtonsoft.Json.JsonConvert.SerializeObject(value));
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "ldtk.LdtkJson, MonoGame.ContentPipeline.UmbrellaToolKit";
        }
    }
}
