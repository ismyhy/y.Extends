using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace y.Extends.WPF.InternalDefine
{
    internal class VisualWrapper : VisualWrapper <Visual>
    {
        public VisualWrapper(Func <FrameworkElement> uiFunc, bool extendDataContext, object dataContent = null, string threadName = "")
        {
            var isloaded = false;
            Loaded += (s, e) =>
            {
                if (isloaded)
                {
                    return;
                }

                isloaded = true;
                var notify = new AutoResetEvent(false);
                var hostVisual = new HostVisual();
                RunThread = new Thread(() =>
                {
                    try
                    {
                        var target = new VisualTarget(hostVisual);
                        var fw = uiFunc?.Invoke() ?? new Grid();

                        CreateDataContextEvent(target, fw, dataContent, extendDataContext);

                        target.RootVisual = fw;
                        notify.Set();
                        Dispatcher.Run();
                    }
                    catch (ThreadAbortException)
                    {
                       //Console.WriteLine(@"异步视图", $"{Thread.CurrentThread.Name} 中断异常");
                    }
                    catch (Exception exception)
                    {
                      //  Log.Error("视图程序", Thread.CurrentThread.Name, exception);
                    }
                })
                {
                    Name = $"UI 子线程  {threadName ?? string.Empty}",
                    IsBackground = true
                };

                RunThread.SetApartmentState(ApartmentState.STA);
                RunThread.Start();
                notify.WaitOne();
                Child = hostVisual;
            };
        }

        public void RunThreadNamed(object name)
        {
            if (RunThread != null)
            {
                RunThread.Name = name?.ToString() ?? "NULL";
            }
        }
    }

    [ContentProperty(nameof (Child))]
    internal class VisualWrapper <T> : FrameworkElement where T : Visual
    {
        private T _child;

        protected Thread RunThread { get; set; }

        public T Child
        {
            get { return _child; }
            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }

                _child = value;

                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get { return _child != null ? 1 : 0; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_child != null && index == 0)
            {
                return _child;
            }

            throw new ArgumentOutOfRangeException(nameof (index));
        }

        protected void CreateDataContextEvent(VisualTarget target, FrameworkElement fw, object dataContent, bool extendDataContext)
        {
            if (extendDataContext)
            {
                DataContextChanged += (ss, ee) =>
                {
                    fw.DataContext = ee.NewValue;
                };
            }

            if (dataContent != null)
            {
                fw.DataContext = target.DataContext = dataContent;
            }
            else
            {
                if (fw.DataContext != null)
                {
                    target.DataContext = fw.DataContext;
                }
                else
                {
                    fw.DataContextChanged += (ss, ee) =>
                    {
                        target.DataContext = ee.NewValue;
                    };
                }
            }
        }
    }



    internal class VisualTarget : PresentationSource
    {
        private readonly System.Windows.Media.VisualTarget _visualTarget;
        private object _dataContext;
        private string _propertyName;

        public VisualTarget(HostVisual hostVisual)
        {
            _visualTarget = new System.Windows.Media.VisualTarget(hostVisual);
        }

        public override Visual RootVisual
        {
            get { return _visualTarget.RootVisual; }
            set
            {
                var oldRoot = _visualTarget.RootVisual;
                _visualTarget.RootVisual = value;
                if (value is FrameworkElement rootFE)
                {
                    rootFE.SizeChanged += root_SizeChanged;
                    rootFE.DataContext = _dataContext;

                    if (_propertyName != null)
                    {
                        var myBinding = new Binding(_propertyName)
                        {
                            Source = _dataContext
                        };
                        rootFE.SetBinding(TextBlock.TextProperty, myBinding);
                    }
                }
                RootChanged(oldRoot, value);
                if (!(value is UIElement rootElement))
                {
                    return;
                }

                rootElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                rootElement.Arrange(new Rect(rootElement.DesiredSize));
            }
        }

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                _dataContext = value;
                if (_visualTarget.RootVisual is FrameworkElement rootElement)
                {
                    rootElement.DataContext = _dataContext;
                }
            }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                _propertyName = value;

                if (_visualTarget.RootVisual is TextBlock rootElement)
                {
                    if (!rootElement.CheckAccess())
                    {
                        throw new InvalidOperationException("Run in Other Thread,Not Ui Thread !!!");
                    }

                    var myBinding = new Binding(_propertyName)
                    {
                        Source = _dataContext
                    };
                    rootElement.SetBinding(TextBlock.TextProperty, myBinding);
                }
            }
        }

        public override bool IsDisposed
        {
            get { return false; }
        }

        public event SizeChangedEventHandler SizeChanged;

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }

        private void root_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var handler = SizeChanged;
            handler?.Invoke(this, e);
        }
    }
}