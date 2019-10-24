namespace LootsCounter
{
    internal class LootsClientAccessor
    {
        protected LootsClient LootsClient { get; private set; }

        protected LootsClientAccessor( LootsClient lootsClient ) {
            LootsClient = lootsClient;
        }
    }
}
