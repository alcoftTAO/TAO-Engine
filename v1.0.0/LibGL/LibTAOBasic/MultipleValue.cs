using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAO.Engine.LibGL.LibTAOBasic
{
    public class MultipleValue<A, B, C, D>
    {
        public A a;
        public B b;
        public C c;
        public D d;

        public MultipleValue(A a, B b, C c, D d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }
}
