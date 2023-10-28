using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectOpen.Patterns.Solvers
{
    public class SolverReflection
    {
        public static List<Type> FindSolvers(Assembly assembly)
        {
            Type genericBaseType = typeof(Solver);
            List<Type> types = new List<Type>();

            foreach (Type item in assembly.DefinedTypes)
            {
                Type derivedType = item;

                if (derivedType.IsSubclassOf(genericBaseType))
                {
                    if (item.IsGenericType) continue;
                    types.Add(item);
                }
            }

            return types;
        }
    }
}