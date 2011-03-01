using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace GLTWarter.Controls
{
    /// <summary>
    /// This panel stacks children vertically and tries to constrain children so that
    /// the panel fits within the available size given by the parent. Only children 
    /// which have the attached property 'Constrain' set to true are constrained.
    /// </summary>
    public class ConstrainingStackPanel : Panel
    {
        private List<UIElement> _constrainableChildren = new List<UIElement>();

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ConstrainingStackPanel),
            new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(OrientationProperty);
            }
            set
            {
                base.SetValue(OrientationProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Constrain.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConstrainProperty =
            DependencyProperty.RegisterAttached("Constrain", typeof(bool), typeof(ConstrainingStackPanel),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        public static bool GetConstrain(DependencyObject obj)
        {
            return (bool)obj.GetValue(ConstrainProperty);
        }

        public static void SetConstrain(DependencyObject obj, bool value)
        {
            obj.SetValue(ConstrainProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            bool isVertical = Orientation == Orientation.Vertical;

            // Desired size for this panel to return to the parent
            double desiredMajor = 0;
            double desiredMinor = 0;

            // Desired heights the two 'types' of children
            double desiredMajorConstrainableChildren = 0;
            double desiredMajorRegularChildren = 0;

            _constrainableChildren.Clear();

            foreach (UIElement child in InternalChildren)
            {
                // Let child figure out how much space it needs
                child.Measure(availableSize);

                if (GetConstrain(child))
                {
                    // Deal with  constrainable children later once we know if they
                    // need to be constrained or not
                    _constrainableChildren.Add(child);
                    desiredMajorConstrainableChildren +=
                        isVertical ? child.DesiredSize.Height : child.DesiredSize.Width;
                }
                else
                {
                    desiredMajorRegularChildren +=
                        isVertical ? child.DesiredSize.Height : child.DesiredSize.Width;
                    desiredMajor +=
                        isVertical ? child.DesiredSize.Height : child.DesiredSize.Width;
                    desiredMinor = Math.Max(desiredMinor, isVertical ? child.DesiredSize.Width : child.DesiredSize.Height);
                }
            }

            // If the desired height of all children exceeds the available height, set the 
            // constrain flag to true
            double desiredMajorAllChildren = desiredMajorConstrainableChildren + desiredMajorRegularChildren;
            bool constrain = desiredMajorAllChildren > (isVertical ? availableSize.Height : availableSize.Width);

            // Holds the space available for the constrainable children to share

            double availableMajorSpace = Math.Max((isVertical ? availableSize.Height : availableSize.Width) - desiredMajorRegularChildren, 0);

            // Re-measure these children and contrain them proportionally, if necessary, so the
            // largest child gets the largest portion of the vertical space available
            foreach (UIElement child in _constrainableChildren)
            {
                if (constrain)
                {
                    double percent = (isVertical ? child.DesiredSize.Height : child.DesiredSize.Width)
                        / desiredMajorConstrainableChildren;
                    double majorSpace = percent * availableMajorSpace;
                    child.Measure(isVertical ? new Size(availableSize.Width, majorSpace) : new Size(majorSpace, availableSize.Height));
                }
                desiredMajor += isVertical ? child.DesiredSize.Height : child.DesiredSize.Width;
                desiredMinor = Math.Max(desiredMinor, isVertical ? child.DesiredSize.Width : child.DesiredSize.Height);
            }

            return isVertical ? new Size(desiredMinor, desiredMajor) : new Size(desiredMajor, desiredMinor);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            bool isVertical = Orientation == Orientation.Vertical;

            double position = 0;
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(isVertical ?
                    new Rect(0, position, finalSize.Width, child.DesiredSize.Height) :
                    new Rect(position, 0, child.DesiredSize.Width, finalSize.Height));
                position += isVertical ? child.DesiredSize.Height : child.DesiredSize.Width;
            }
            return finalSize;
        }
    }
}
