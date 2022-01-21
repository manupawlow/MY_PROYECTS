using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEditor.Effects
{
    public class NoneEffect : Effect
    {
        public NoneEffect(object[] config) : base(config)
        {
        }

        public override Bitmap ApplyEffect(Bitmap bitmap) => bitmap;
    }
}
