﻿using TGC.Core.Text;
using System.Drawing;

namespace TGC.Group.Model
{
    class Text
    {
        public static TgcText2D newText(string formato, int x, int y)
        {
            TgcText2D texto = new TgcText2D();
            texto.Text = formato;
            texto.Position = new Point(x, y);
            texto.Size = new Size(0, 0);
            texto.Color = Color.Gold;
            return texto;
        }
    }
}
