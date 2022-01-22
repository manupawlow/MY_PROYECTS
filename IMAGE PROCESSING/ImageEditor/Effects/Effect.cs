using ImageEditor.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ImageEditor
{
    public abstract class Effect
    {
        public bool IsDynamic = false;

        public object[] Configuration { get; set; }

        public Effect(object[] config)
        {
            Configuration = config;
        }

        public static Effect GetEffect(string effectConfig)
        {
            //effectName:1,2,3
            var splited = effectConfig.Split(':');
            var effectName = splited[0];
            object[] config = splited.Length > 1 ? splited[1].Split(',') : null;

            var effectNameType = "ImageEditor.Effects." + effectName[0].ToString().ToUpper() + effectName.ToLower().Substring(1) + "Effect";

            Type type = Type.GetType(effectNameType, throwOnError: false);

            return (Effect)Activator.CreateInstance(type, new object[] { config });
        }

        public Bitmap ApplyEffect(Image img) => ApplyEffect((Bitmap)img);

        public abstract Bitmap ApplyEffect(Bitmap bitmap);

        public static List<Type> GetAllEffects() =>
            System.Reflection.Assembly.GetAssembly(typeof(Effect)).GetTypes().Where(t => t.IsSubclassOf(typeof(Effect))).ToList();
    }
}

