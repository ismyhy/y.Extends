using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using y.Extends.WPF.InternalDefine;

namespace y.Extends.WPF.Controls
{
    public class AsyncControl : DockPanel
    {
        public static readonly DependencyProperty AsyncRunNameProperty = DependencyProperty.Register("AsyncRunName", typeof (string), typeof (AsyncControl), new PropertyMetadata(default (string), AysncRunNamePropretyChangedCallback));
        public static readonly DependencyProperty ExtendsParentDataContextProperty = DependencyProperty.Register("ExtendsParentDataContext", typeof (bool), typeof (AsyncControl), new PropertyMetadata(true, ExtendsParentDataContextPropertyCallback));
        private readonly AutoResetEvent asyncRunResetEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent extendResetEvent = new AutoResetEvent(false);
        private bool isRender;

        //public static readonly DependencyProperty ChildTypeProperty = DependencyProperty.Register("ChildType", typeof (Type), typeof (AsyncControl), new PropertyMetadata(default (Type)));

        /// <summary>
        ///     构建异步视窗
        ///     <para>childType 子控件类型</para>
        ///     <para>@params 构造子控件的构造参数</para>
        /// </summary>
        /// <param name="childType">子控件类型</param>
        /// <param name="params">构造子控件的构造参数</param>
        public AsyncControl(Type childType, params object[] @params) : this()
        {
            ChildType = childType ?? throw new ArgumentException("子控件类型不能为空");
            Params = @params;
            ExtendsParentDataContext = false;
        }

        /// <summary>
        ///     构建异步视窗
        ///     <para>extendsDataContext 是否继承父类DataContext</para>
        ///     <para>threadDescription 该异步视窗的描述</para>
        /// </summary>
        /// <param name="extendsDataContext">是否继承父类DataContext</param>
        /// <param name="threadDescription">该异步视窗的描述</param>
        public AsyncControl(bool extendsDataContext, string threadDescription) : this()
        {
            ExtendsParentDataContext = extendsDataContext;
            AsyncRunName = threadDescription ?? Guid.NewGuid().ToString();
            ExtendsParentDataContext = false;
        }

        public AsyncControl()
        {
            SizeChanged += (s, e) =>
            {
                if (ExtendsParentDataContext && DataContext == null)
                {
                    return;
                }

                RenderViewer();
            };
            var a = new AutoResetEvent(false);

            Loaded += (s, e) =>
            {
                if (ExtendsParentDataContext)
                {
                    Task.Run(() =>
                    {
                        a.WaitOne();
                        Dispatcher.BeginInvoke((Action) RenderViewer);
                    });
                    return;
                }

                RenderViewer();
            };

            DataContextChanged += (s, e) =>
            {
                if (ExtendsParentDataContext && e.NewValue != null)
                {
                    if (ChildValue != null)
                    {
                        ChildValue.DataContext = e.NewValue;
                    }
                    a.Set();
                }
            };
        }

        public string AsyncRunName
        {
            get { return (string) GetValue(AsyncRunNameProperty); }
            set { SetValue(AsyncRunNameProperty, value); }
        }

        /// <summary>
        ///     是否继承 父容器的数据源 DataContext
        /// </summary>
        public bool ExtendsParentDataContext
        {
            get { return (bool) GetValue(ExtendsParentDataContextProperty); }
            set { SetValue(ExtendsParentDataContextProperty, value); }
        }

        private object[] Params { get; }

        public Type ChildType { get; set; }

        private VisualWrapper ChildValue { get; set; }

        private static void AysncRunNamePropretyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (! (d is AsyncControl ac))
            {
                return;
            }

            ac.ChildValue?.RunThreadNamed(e.NewValue);
        }

        private static void ExtendsParentDataContextPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (! (d is AsyncControl ac))
            {
                return;
            }

            ac.extendResetEvent.Set();
        }

        private void RenderViewer()
        {
            if (isRender)
            {
                return;
            }

            isRender = true;
            if (ActualHeight <= 0 || ActualWidth <= 0)
            {
                throw new ArgumentException($"未设置 {nameof (ActualHeight)} 或 {nameof (ActualWidth)}");
            }

            var w = ActualWidth;
            var h = ActualHeight;

            ChildValue = new VisualWrapper(() =>
            {
                if (! (Activator.CreateInstance(ChildType, Params) is FrameworkElement fe))
                {
                    return new Grid();
                }

                fe.Width = w;
                fe.Height = h;
                return fe;
            }, ExtendsParentDataContext, ExtendsParentDataContext ? DataContext : null, AsyncRunName ?? ChildType.ToString());

            Children.Add(ChildValue);
        }
    }
}