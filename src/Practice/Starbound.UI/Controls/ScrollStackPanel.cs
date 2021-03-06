﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Starbound.UI.Controls
{
    public class ScrollStackPanel : Panel, IEnumDrawableUIElemt
    {
        private Orientation orientation;

        private List<UIElement> showChildren { get; set; }
        public ReadOnlyCollection<UIElement> ShowChildren
        {
            get
            {
                return showChildren.AsReadOnly();
            }
        }

        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation == value) { return; }
                orientation = value;
                UpdateLayout();
            }
        }

        private double _Begin;
        public double Begin 
        { 
            get
            {
                return _Begin;
            }
            set
            {
                _Begin = value < 0.0 ? 0.0 : value;
                UpdateLayout();
            }
        }
        public double FirstBeginPos { get; set; }
        public Vector2 BeginPos { get; set; }
        public bool IsScroll { get; set; }

        public ScrollStackPanel()
        {
            showChildren = new List<UIElement>();
        }

        public override void UpdateLayout()
        {
            showChildren.Clear();
            if (Orientation == Orientation.Horizontal) { UpdateLayoutHorizontal(); }
            else { UpdateLayoutVertical(); }
        }

        private void UpdateLayoutHorizontal()
        {
            double[] sizes = new double[Children.Count];

            // Allocate sizes up until each child has at least their minimum size.
            // Stop early if you run out of space to allocate.
            for (int index = 0; index < Children.Count; index++)
            {
                sizes[index] = Children[index].MinimumWidth + Children[index].Margin.TotalHorizontal;
            }

            // Allocate more space to each child until each child has their preferred size.
            // Stop early if you run out of space to allocate.
            for (int index = 0; index < Children.Count; index++)
            {
                sizes[index] = Children[index].PreferredWidth - Children[index].MinimumWidth;
            }

            double x = X;
            // Give each child their allocated space.
            for (int index = 0; index < Children.Count; index++)
            {
                Children[index].ActualHeight = Children[index].PreferredHeight;
                Children[index].ActualWidth = sizes[index] - Children[index].Margin.TotalHorizontal;
                Children[index].X = x + Children[index].Margin.Left;
                Children[index].Y = Y + Children[index].Margin.Top;
                Children[index].X -= Begin;
                x += sizes[index];

                if (Children[index].X + Children[index].ActualWidth > 0 
                    && Children[index].X + Children[index].ActualWidth < this.ActualWidth)
                {
                    showChildren.Add(Children[index]);
                }
            }
        }


        private void UpdateLayoutVertical()
        {
            double[] sizes = new double[Children.Count];

            // Allocate sizes up until each child has at least their minimum size.
            // Stop early if you run out of space to allocate.
            for (int index = 0; index < Children.Count; index++)
            {
                sizes[index] = Children[index].MinimumHeight + Children[index].Margin.TotalVertical;
            }

            // Allocate more space to each child until each child has their preferred size.
            // Stop early if you run out of space to allocate.
            for (int index = 0; index < Children.Count; index++)
            {
                sizes[index] = Children[index].PreferredHeight - Children[index].MinimumHeight;
            }

            double y = Y;
            // Give each child their allocated space.
            for (int index = 0; index < Children.Count; index++)
            {
                Children[index].ActualWidth = Children[index].PreferredWidth;
                Children[index].ActualHeight = sizes[index] - Children[index].Margin.TotalVertical;
                Children[index].X = X + Children[index].Margin.Left;
                Children[index].Y = y + Children[index].Margin.Top;
                Children[index].Y -= Begin;
                y += sizes[index];

                if (Children[index].Y + Children[index].ActualHeight > 0 && Children[index].Y + Children[index].ActualHeight < this.ActualHeight)
                {
                    showChildren.Add(Children[index]);
                }
            }
        }

        protected override void DispatchClick(int x, int y)
        {
            if(!IsScroll)
            {
                foreach (var obj in ShowChildren)
                {
                    if (x > obj.X && x < obj.X + obj.ActualWidth &&
                        y > obj.Y && y < obj.Y + obj.ActualHeight)
                    {
                        var click = clickActions[obj];
                        if (click != null)
                        {
                            click();
                        }
                    }
                }
            }
        }

        public IEnumerable<UIElement> GetDrawableElemt()
        {
            return ShowChildren;
        }

        public override bool OnMousePressed(int x, int y)
        {
            base.OnMousePressed(x, y);
            if(IsFocus)
            {
                FirstBeginPos   = Begin;
                BeginPos        = new Vector2(x, y);
            }
            return IsFocus;
        }

        public override bool OnMouseMoved(int x, int y)
        {
            if(IsFocus)
            {
                IsScroll = true;
                if(orientation == Controls.Orientation.Horizontal)
                {
                    MoveBeginHorizontal(x, y);
                }
                else
                {
                    MoveBeginVertical(x, y);
                }
            }
            return IsFocus;
        }

        public override bool OnMouseReleased(int x, int y)
        {

            bool result = base.OnMouseReleased(x, y);
            IsScroll    = false;
            return result;
        }

        private void MoveBeginHorizontal(int x, int y)
        {
            Begin = FirstBeginPos + BeginPos.X - x;
        }

        private void MoveBeginVertical(int x, int y)
        {
            Begin = FirstBeginPos + BeginPos.Y - y;
        }
    }
}
