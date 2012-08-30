using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Oqat.ViewModel.MacroPlugin
{
    class HighLight_Adorner : Adorner
    {

        private AdornerLayer adornerLayer;
        private Size size;
        private double opacity;
        Point position;

   
        public HighLight_Adorner(UIElement adornedElement,
            Point startPosition, Size size,double opacity,
            AdornerLayer adornerLayer)
            : base(adornedElement)
        {

            this.opacity = opacity;
            this.size = size;
            this.adornerLayer = adornerLayer;
            adornerLayer.Add(this);
            this.position = startPosition;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(position, size);
            drawingContext.PushOpacity(opacity);
                drawingContext.DrawRectangle(SystemColors.HighlightBrush, null, adornedElementRect);
        }

        public void UpdateSizePosition(Point position, Size size)
        {
            this.position = position;
            this.size = size;
            if (adornerLayer != null)
                adornerLayer.Update(this.AdornedElement);
        }
    }
}
