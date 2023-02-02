using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using MiAPR_1.Models;
using System.Linq;

namespace MiAPR_1.CustomControls
{
    public class VectorField : Control
    {

        internal void SetVectors(List<VectorModel> vectors)
        {
            _vectorList = vectors;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
             new Action(
                  delegate ()
                   {
                       this.InvalidateVisual();
                   }));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Background, null, new Rect() { X = 0, Y = 0, 
                Height = ActualHeight, Width = ActualWidth });
            if(_vectorList != null)
            {
                double maxX = 0, maxY = 0;
                _vectorList.ForEach(vector =>
                {
                    if (maxX < vector.X) maxX = vector.X;
                    if (maxY < vector.Y) maxY = vector.Y;
                });
                double actualW = ActualWidth;
                double actualH = ActualHeight;
                foreach (var vector in _vectorList)
                {
                    drawingContext.DrawEllipse(vector.Brush, null, new Point { X = ((double)vector.X/maxX)*actualW,
                        Y = ((double)vector.Y / maxY) * actualH}, DOT_RADIUS, DOT_RADIUS);
                }
                
            }
        }

        private List<VectorModel> _vectorList;

        private const int DOT_RADIUS = 3;
    }
}
