using System.Threading;

namespace Optional.Async.Tests
{
    public class TestSynchronizationContext : SynchronizationContext
    {
        private int _called = 0;

        public int Called => _called;

        public override void Post(SendOrPostCallback d, object state)
        {
            Interlocked.Increment(ref _called);
            base.Post(d, state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Interlocked.Increment(ref _called);
            base.Send(d, state);
        }
    }
}
