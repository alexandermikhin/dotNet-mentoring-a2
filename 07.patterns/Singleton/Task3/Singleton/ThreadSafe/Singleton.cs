namespace Singleton.ThreadSafe
{
    public sealed class Singleton
    {
        static readonly Singleton instance = new Singleton();

        Singleton() { }

        public static Singleton Instance => instance;
    }
}
