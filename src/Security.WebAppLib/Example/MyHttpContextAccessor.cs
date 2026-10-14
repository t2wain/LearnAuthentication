namespace Security.WebAppLib.Example
{
    /// <summary>
    /// Provide an HttpContext for testing
    /// </summary>
    public class MyHttpContextAccessor : IHttpContextAccessor
    {
        IServiceProvider _provider;

        public MyHttpContextAccessor(IServiceProvider provider)
        {
            this._provider = provider;
            DefaultHttpContext = 
                new DefaultHttpContext() { RequestServices = _provider };
        }

        protected HttpContext DefaultHttpContext { get; set; }

        public HttpContext? MyHttpContext { protected get; set; }

        public HttpContext HttpContext { 
            get => MyHttpContext ?? DefaultHttpContext; 
            set
            {
                MyHttpContext = value;
                if (MyHttpContext != null)
                    MyHttpContext.RequestServices = _provider;
            }
        }
    }
}
