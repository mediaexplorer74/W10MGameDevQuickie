using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Win8.Core.MultiBinding
{
    /// <summary>
    /// Provides a mechanism for attaching a MultiBinding to an element
    /// </summary>
    public class BindingUtil
    {
        //DataContextPiggyBack attached property

        /// <summary>
        /// DataContextPiggyBack Attached Dependency Property, used as a mechanism for exposing
        /// DataContext changed events
        /// </summary>
        public static readonly DependencyProperty DataContextPiggyBackProperty =
            DependencyProperty.RegisterAttached("DataContextPiggyBack", 
                typeof (object), typeof (BindingUtil),
                new PropertyMetadata(null, OnDataContextPiggyBackChanged));

        public static object GetDataContextPiggyBack(DependencyObject d)
        {
            return d.GetValue(DataContextPiggyBackProperty);
        }

        public static void SetDataContextPiggyBack(DependencyObject d, object value)
        {
            d.SetValue(DataContextPiggyBackProperty, value);
        }

        /// <summary>
        /// Handles changes to the DataContextPiggyBack property.
        /// </summary>
        private static void OnDataContextPiggyBackChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement targetElement = d as FrameworkElement;

            // whenever the targeElement DataContext is changed, copy the updated property
            // value to our MultiBinding.
            MultiBindings relay = GetMultiBindings(targetElement);
            relay.SetDataContext(targetElement.DataContext);
        }

  

        //MultiBindings attached property

        public static MultiBindings GetMultiBindings(DependencyObject obj)
        {
            return (MultiBindings) obj.GetValue(MultiBindingsProperty);
        }

        public static void SetMultiBindings(DependencyObject obj, MultiBindings value)
        {
            obj.SetValue(MultiBindingsProperty, value);
        }

        public static readonly DependencyProperty MultiBindingsProperty =
            DependencyProperty.RegisterAttached("MultiBindings",
                typeof (MultiBindings), typeof (BindingUtil), 
                new PropertyMetadata(null, OnMultiBindingsChanged));

        /// <summary>
        /// Invoked when the MultiBinding property is set on a framework element
        /// </summary>
        private static void OnMultiBindingsChanged(DependencyObject depObj, 
            DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement targetElement = depObj as FrameworkElement;

            // bind the target elements DataContext, to our DataContextPiggyBack property
            // this allows us to get property changed events when the targetElement
            // DataContext changes
            targetElement.SetBinding(DataContextPiggyBackProperty, new Binding());

            MultiBindings bindings = GetMultiBindings(targetElement);

            bindings.Initialize(targetElement);
        }

  
    }
}
