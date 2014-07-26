// 1) Formally, a monad consists of a type constructor M and two operations, bind and return.
//
// 2) The operations must fulfill several properties to allow the correct composition of monadic functions
//   (i.e. functions that use values from the monad as their arguments or return value).
//
// 3) In most contexts, a value of type M a can be thought of as an action that returns a value of type a.
//
// 4) The return operation takes a value from a plain type a and puts it into a monadic container of type M a.
//
// 5) The bind operation chains a monadic value of type M a with a function of type a -> M b to create a monadic
//   value of type M b, effectively creating an action that chooses the next action based on the results of
//   previous actions.

namespace MonadDemo
{
    public class Monad<T>
    {
        private readonly T _value;

        public Monad(T value)
        {
            _value = value;
        }

        public T Value
        {
            get { return _value; }
        }
    }

    //public class Program
    //{
    //    public static void Main()
    //    {
            
    //    }
    //}
}