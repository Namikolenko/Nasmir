using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Repository
{
    class Factory
    {
        private static Factory instance;

        private Factory() { }
        public static Factory Instance
        {
            get
            {
                // Lazy initialization; initialization on demand
                if (instance == null)
                    instance = new Factory();
                return instance;
            }
        }

    }
}
