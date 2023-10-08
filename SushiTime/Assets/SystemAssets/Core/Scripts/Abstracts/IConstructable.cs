namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Interface for constructing a new object.
    /// </summary>
    public interface IConstructable<T1, T2, T3>
    {
        public void ConstructWithThree
            (T1 param1, T2 param2, T3 param3);
    }
}