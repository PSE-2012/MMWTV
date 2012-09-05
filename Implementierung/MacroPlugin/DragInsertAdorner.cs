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
    class DragInsertAdorner : Adorner
    {

        private double leftOffset;
        private double topOffset;
        private AdornerLayer adornerLayer;
        private ContentPresenter _ContentPresenter;

        public  DragInsertAdorner(UIElement adornedElement, 
            Point startPosition, 
            Visual content, 
            AdornerLayer adornerLayer) : base(adornedElement)
        {
            
            this.adornerLayer = adornerLayer;
            
            _Visuals = new VisualCollection(this);
            _ContentPresenter = new ContentPresenter();
            _Visuals.Add(_ContentPresenter);
            this.Content = content;
            adornerLayer.Add(this);
            UpdatePosition(startPosition.X, startPosition.Y);
        }

          private VisualCollection _Visuals;



  protected override Size MeasureOverride(Size constraint)
  {
    _ContentPresenter.Measure(constraint);
    return _ContentPresenter.DesiredSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
      _ContentPresenter.Arrange(new Rect(finalSize));
      return finalSize;
  }

  public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
  {
      GeneralTransformGroup result = new GeneralTransformGroup();
      result.Children.Add(base.GetDesiredTransform(transform));
      result.Children.Add(new TranslateTransform(leftOffset, topOffset));
      return result;
  }

  public void UpdateSize(double height, double widht)
  {
      if(height>0)
         this.Height = height;
      if (widht > 0)
          this.Width = widht;

      if (adornerLayer != null)
          adornerLayer.Update(this.AdornedElement);
  }

  public void UpdatePosition(double left, double top)
  {
      leftOffset = left;
      topOffset = top;
      if (adornerLayer != null)
          adornerLayer.Update(this.AdornedElement);
  }

  protected override Visual GetVisualChild(int index)
  { return _Visuals[index]; }

  protected override int VisualChildrenCount
  { get { return _Visuals.Count; } }

  public object Content
  {
    get { return _ContentPresenter.Content; }
    set { _ContentPresenter.Content = value; }
  }
        
    }
}
