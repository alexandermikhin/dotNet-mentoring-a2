using System;

namespace Singleton.NonThreadSafe
{
    public class Singleton
    {
        static Singleton instance;

        Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }

                return instance;
            }
        }
    }
}
