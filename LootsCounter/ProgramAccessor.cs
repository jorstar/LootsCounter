namespace LootsCounter
{
    internal class ProgramAccessor
    {
        protected Program Program { get; private set; }

        protected ProgramAccessor( Program program ) {
            Program = program;
        }
    }
}
