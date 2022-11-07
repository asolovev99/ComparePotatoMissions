using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComparePotatoMissions
{
    class ClassStyles
    {
        static public Style styleButton;

        static ClassStyles() 
        {
            /*
            <Style TargetType="Button">
                <Setter Property="Control.FontSize" Value="18" />
                <Setter Property="Control.FontWeight" Value="Bold" />
                <Setter Property="Control.HorizontalAlignment" Value="Left" />
                <Setter Property="Control.VerticalAlignment" Value="Top" />
                <Setter Property="Control.Height" Value="40" />
            </Style>
             */

            styleButton = new Style();
            styleButton.Setters.Add(new Setter { Property = Control.FontSizeProperty, Value = 18 });
            //styleButton.Setters.Add(new Setter { Property = Control.FontWeightProperty, Value = FontWeight.FromOpenTypeWeight( });
            //styleButton.Setters.Add(new Setter { Property = Control.HorizontalAlignmentProperty, Value = HorizontalAlignment.Left });
            //styleButton.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
            //styleButton.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
        }
    }
}
