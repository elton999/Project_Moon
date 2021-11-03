using System;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Ldtk
{
    public class Ldtk : ContentTypeReader<ldtk.LdtkJson>
    {
        protected override ldtk.LdtkJson Read(ContentReader input, ldtk.LdtkJson existingInstance)
        {
            try
            {
                string text = input.ReadString();
                return ldtk.LdtkJson.FromJson(text);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new ldtk.LdtkJson();
            }
        }
    }
}
