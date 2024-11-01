namespace CodebridgeTest.Middlewares
{
    public class RateLimitMiddleware
    {
        private static int _requestCount;
        private static readonly object _lock = new object();
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            lock (_lock)
            {
                if (_requestCount >= 10)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return;
                }
                _requestCount++;
            }

            await _next(context);

            lock (_lock)
            {
                _requestCount--;
            }
        }
    }

}
